
using Dapper;
using Domain.Interfaces.Repository;
using Infra.CrossCutting.Exceptions.DataExceptions;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;

namespace Infra.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        IDbConnection _connection;

        readonly string connectionString = "";
        public BaseRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task Add(TEntity entity)
        {
            int rowsEffected = 0;

            try
            {
                string tableName = BaseRepository<TEntity>.GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                rowsEffected = await _connection.ExecuteAsync(query, entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = BaseRepository<TEntity>.GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                rowsEffected = await _connection.ExecuteAsync(query, id);
            }
            catch (Exception ex) { }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            IEnumerable<TEntity>? result = null;
            try
            {
                string tableName = BaseRepository<TEntity>.GetTableName();
                string query = $"SELECT * FROM {tableName}";

                result = await _connection.QueryAsync<TEntity>(query);
            }
            catch (Exception ex) { }

            return result;
        }

        public async Task<TEntity> GetById(Guid id)
        {
            IEnumerable<TEntity>? result = null;
            try
            {
                string tableName = BaseRepository<TEntity>.GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{id}'";

                result = await _connection.QueryAsync<TEntity>(query);
            }
            catch (Exception ex) { }

            return result.FirstOrDefault();
        }

        public async Task Update(TEntity entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = BaseRepository<TEntity>.GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                    string propertyName = property.Name;
                    string columnName = columnAttr.Name;

                    query.Append($"{columnName} = @{propertyName},");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                rowsEffected = await _connection.ExecuteAsync(query.ToString(), entity);
            }
            catch (Exception) { }
        }

        private static string GetTableName()
        {
            var type = typeof(TEntity);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                string tableName = tableAttr.Name;
                return tableName;
            }

            throw new TableNameNotFoundException($"O atribulo TableName nao foi encontrado para classe { type.Name }");
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }


        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(TEntity);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(TEntity).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(TEntity).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(TEntity).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties != null && properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        private readonly IBaseRepository<TEntity> _baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public TEntity Add<TValidator>(TEntity obj) where TValidator : AbstractValidator<TEntity>
        {
            Validate(obj, Activator.CreateInstance<TValidator>());

            _baseRepository.Add(obj);
            return obj;
        }

        public async Task Delete(Guid id) => await _baseRepository.Delete(id);

        public async Task<IEnumerable<TEntity>> GetAll() => await _baseRepository.GetAll();

        public async Task<TEntity> GetById(Guid id) => await _baseRepository.GetById(id);

        public async Task Update<TValidator>(TEntity obj) where TValidator : AbstractValidator<TEntity>
        {
            Validate(obj, Activator.CreateInstance<TValidator>());
            await _baseRepository.Update(obj);
        }

        private void Validate(TEntity obj, AbstractValidator<TEntity> validator)
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }
    }
}

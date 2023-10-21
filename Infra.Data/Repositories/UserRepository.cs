using Domain.Entities.UserEntities;
using Domain.Interfaces.Repository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public void Test(string algo)
        { 
            if(algo.IsNullOrEmpty())
            {
                throw new Exception("String vazia");
            }
        }
    }
}

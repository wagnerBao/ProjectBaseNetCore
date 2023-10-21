using Domain.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        void Test(string algo) { }
    }
}

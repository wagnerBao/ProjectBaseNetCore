using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Infra.Data;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Service.User;

namespace Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigInjections(IServiceCollection serviceCollection)
        {
            //Repositories
            serviceCollection = AddRepositories(serviceCollection);

            //Services
            serviceCollection = AddServices(serviceCollection);

            return serviceCollection;
        }

        public static IServiceCollection AddRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

            return serviceCollection;
        }

        public static IServiceCollection AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();

            return serviceCollection;
        }
    }
}
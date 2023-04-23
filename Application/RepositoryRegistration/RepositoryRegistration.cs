using Infrastructure.Repositories.CustomerRepository;
using Infrastructure.Repositories.OrderRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryRegistration
{
    public static class RepositoryRegistration
    {
        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}

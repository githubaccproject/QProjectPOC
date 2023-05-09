using Infrastructure.Repositories.CustomerRepository;
using Infrastructure.Repositories.OrderRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServicesRegistration
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

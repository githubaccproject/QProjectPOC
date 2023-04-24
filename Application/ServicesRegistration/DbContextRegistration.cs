using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServicesRegistration
{

    public static class DbContextRegistration
    {
        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}

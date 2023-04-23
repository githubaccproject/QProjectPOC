using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.RepositoryRegistration
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

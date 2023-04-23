using Application.Handlers.CustomerHandler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Handlers
{
    public static class MediatRRegistration
    {
        public static void RegisterMediatRHandlers(IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetCustomersQueryHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateCustomerCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(UpdateCustomerCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DeleteCustomerCommandHandler)));
        }
    }
}

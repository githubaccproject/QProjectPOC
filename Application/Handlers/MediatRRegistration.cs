using Application.Handlers.CustomerHandler;
using Application.Handlers.OrderHandler;
using Application.Queries.OrderQuery;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Handlers
{
    public static class MediatRRegistration
    {
        public static void RegisterMediatRHandlers(IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetCustomersQueryHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetCustomerByIdQueryHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateCustomerCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(UpdateCustomerCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DeleteCustomerCommandHandler)));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetOrdersQueryHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetOrderByIdQueryHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateOrderCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(UpdateOrderCommandHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DeleteOrderCommandHandler)));
        }
    }
}

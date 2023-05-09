using Application.Customers.Dtos;
using Application.Customers.Validators;
using Application.Orders.Dtos;
using Application.Orders.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServicesRegistration
{
    public static class ValidatorRegistration
    {
        public static void RegisterValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateCustomerDto>, CreateCustomerDtoValidator>();
            services.AddTransient<IValidator<UpdateCustomerDto>, UpdateCustomerDtoValidator>();
            services.AddTransient<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
            services.AddTransient<IValidator<UpdateOrderDto>, UpdateOrderDtoValidator>();
        }
    }
}

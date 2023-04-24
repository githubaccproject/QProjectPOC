using Application.DTOs;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
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

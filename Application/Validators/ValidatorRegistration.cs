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
            services.AddTransient<IValidator<CreateCustomerDto>, CustomerDtoValidator>();
            services.AddTransient<IValidator<UpdateCustomerDto>, UpdateCustomerDtoValidator>();
        }
    }
}

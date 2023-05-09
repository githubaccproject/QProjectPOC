using Application.Orders.Dtos;
using FluentValidation;


namespace Application.Orders.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(dto => dto.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(dto => dto.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(dto => dto.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be a positive value.");

            RuleForEach(dto => dto.OrderProducts)
                .SetValidator(new CreateOrderProductDtoValidator());
        }
    }

    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {

            RuleFor(dto => dto.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(dto => dto.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(dto => dto.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be a positive value.");

            RuleForEach(dto => dto.OrderProducts)
                .SetValidator(new UpdateOrderProductDtoValidator());
        }
    }
}

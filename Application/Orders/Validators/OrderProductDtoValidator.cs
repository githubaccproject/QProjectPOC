using Application.Orders.Dtos;
using FluentValidation;

namespace Application.Orders.Validators
{

    public class CreateOrderProductDtoValidator : AbstractValidator<CreateOrderProductDto>
    {
        public CreateOrderProductDtoValidator()
        {
            RuleFor(dto => dto.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(dto => dto.OrderId)
                .NotEmpty().WithMessage("Order ID is required.");

            RuleFor(dto => dto.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be a positive value.");

            RuleFor(dto => dto.MembershipName)
                .MaximumLength(100).WithMessage("MembershipName must not exceed 100 characters.");

            RuleFor(dto => dto.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be a positive value.");
        }
    }


    public class UpdateOrderProductDtoValidator : AbstractValidator<UpdateOrderProductDto>
    {
        public UpdateOrderProductDtoValidator()
        {
            RuleFor(dto => dto.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(dto => dto.OrderId)
               .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(dto => dto.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be a positive value.");

            RuleFor(dto => dto.MembershipName)
                .MaximumLength(100).WithMessage("MembershipName must not exceed 100 characters.");

            RuleFor(dto => dto.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be a positive value.");
        }
    }
}

using Application.Customers.Dtos;
using MediatR;


namespace Application.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<int>
    {
        public CreateCustomerDto Customer { get; set; }
    }

    public class UpdateCustomerCommand : IRequest<int>
    {
        public int Id { get; set; }
        public UpdateCustomerDto Customer { get; set; }
    }

    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

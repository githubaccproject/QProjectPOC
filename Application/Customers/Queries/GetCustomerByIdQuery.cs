using Application.Customers.Dtos;
using MediatR;

namespace Application.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }

        public GetCustomerByIdQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}

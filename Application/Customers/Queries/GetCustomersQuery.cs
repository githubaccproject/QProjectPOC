using Application.Customers.Dtos;
using MediatR;

namespace Application.Customers.Queries
{
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {

    }
}

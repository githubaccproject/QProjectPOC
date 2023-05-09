using Application.Orders.Dtos;
using MediatR;

namespace Application.Orders.Queries
{
    public class GetOrdersQuery : IRequest<List<OrderDto>>
    {
        
    }
}

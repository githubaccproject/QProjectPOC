using Application.Orders.Dtos;
using MediatR;


namespace Application.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<int>
    {

        public int Id { get; set; }
        public UpdateOrderDto Order { get; set; }
    }

    public class CreateOrderCommand : IRequest<int>
    {
        public CreateOrderDto Order { get; set; }
    }

    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}

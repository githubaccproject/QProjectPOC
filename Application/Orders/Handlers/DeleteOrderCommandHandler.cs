using Application.Orders.Commands;
using Infrastructure.Repositories.OrderRepository;
using MediatR;

namespace Application.Orders.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _OrderRepository;

        public DeleteOrderCommandHandler(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {

            var Order = await _OrderRepository.GetByIdAsync(request.Id);

            if (Order == null)
            {

                throw new Exception("Order not found.");
            }


            _OrderRepository.Delete(Order);
            await _OrderRepository.SaveChangesAsync();

            return true;
        }
    }
}

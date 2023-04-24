using Application.Commands;
using Infrastructure.Repositories.OrderRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OrderHandler
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
            // Retrieve the Order from the repository
            var Order = await _OrderRepository.GetByIdAsync(request.Id);

            if (Order == null)
            {
                // Order not found, handle appropriately
                throw new Exception("Order not found.");
            }

            // Delete the Order from the repository
            _OrderRepository.Delete(Order);
            await _OrderRepository.SaveChangesAsync();

            return true;
        }
    }
}

using Application.Exceptions;
using Application.Orders.Commands;
using Domain.Entities;
using Infrastructure.Repositories.OrderRepository;
using MediatR;

namespace Application.Orders.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            // Get the existing order from the repository
            var existingOrder = await _orderRepository.GetByIdAsync(request.Id);

            if (existingOrder == null)
            {
                throw new NotFoundException("Order not found");
            }
            else
            {
                _orderRepository.Detached(existingOrder);
            }


            existingOrder.Customer_ID = request.Order.CustomerId;
            existingOrder.Date = request.Order.Date;
            existingOrder.price = request.Order.Price;

            if (request.Order.OrderProducts != null)
            {
                var orderProductsToRemove = existingOrder.OrderProducts
                    .Where(op => !request.Order.OrderProducts.Any(opd => opd.Id == op.Id))
                    .ToList();

                foreach (var orderProductToRemove in orderProductsToRemove)
                {
                    existingOrder.OrderProducts.Remove(orderProductToRemove);
                }


                foreach (var orderProductDto in request.Order.OrderProducts)
                {
                    var existingOrderProduct = existingOrder.OrderProducts.FirstOrDefault(op => op.Id == orderProductDto.Id);

                    if (existingOrderProduct != null)
                    {

                        existingOrderProduct.Product_ID = orderProductDto.ProductId; // Updated property name from Product_ID to ProductId
                        existingOrderProduct.price = orderProductDto.Price;
                        existingOrderProduct.MembershipName = orderProductDto.MembershipName;
                        existingOrderProduct.Quantity = orderProductDto.Quantity;
                    }
                    else
                    {

                        var newOrderProduct = new OrderProduct
                        {
                            Order_ID = existingOrder.Id,
                            Product_ID = orderProductDto.ProductId,
                            price = orderProductDto.Price,
                            MembershipName = orderProductDto.MembershipName,
                            Quantity = orderProductDto.Quantity
                        };
                        existingOrder.OrderProducts.Add(newOrderProduct);
                    }
                }
            }

            _orderRepository.Update(existingOrder);

            return existingOrder.Id;
        }
    }

}

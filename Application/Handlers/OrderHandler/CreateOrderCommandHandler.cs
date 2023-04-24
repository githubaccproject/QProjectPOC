﻿using Application.Commands;
using Domain.Entities;
using Infrastructure.Repositories.OrderRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OrderHandler
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                
                Customer_ID = request.Order.CustomerId, 
                Date = request.Order.Date,
                price = request.Order.Price, 
                OrderProducts = request.Order.OrderProducts.Select(op => new OrderProduct
                {
                  
                    Product_ID = op.ProductId, 
                    price = op.Price,
                    MembershipName = op.MembershipName,
                    Quantity = op.Quantity,
                    Order = null
                }).ToList()
            };
            await _orderRepository.AddAsync(order);
            return order.Id;
        }
    }

}

﻿using Application.DTOs;
using Application.Exceptions;
using Application.Queries.OrderQuery;
using AutoMapper;
using Infrastructure.Repositories.OrderRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OrderHandler
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // Get the order from the repository
            var order = await _orderRepository.GetByIdAsync(request.Id);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            // Map the Order entity to OrderDto
            var orderDto = _mapper.Map<OrderDto>(order);

            return orderDto;
        }
    }

}

using Application.Orders.Dtos;
using Application.Orders.Queries;
using AutoMapper;
using Infrastructure.Repositories.OrderRepository;
using MediatR;

namespace Application.Orders.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderRepository OrderRepository, IMapper mapper)
        {
            _OrderRepository = OrderRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var Orders = _OrderRepository.GetAll();
            var OrderDtos = _mapper.Map<List<OrderDto>>(Orders);
            return OrderDtos;
        }
    }
}

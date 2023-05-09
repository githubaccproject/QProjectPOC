using Application.Customers.Dtos;
using Application.Orders.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>().ReverseMap();

            CreateMap<CreateOrderDto, Order>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<UpdateOrderDto, Order>().ReverseMap();

        }
    }

}

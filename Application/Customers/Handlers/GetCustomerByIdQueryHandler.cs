using Application.Customers.Dtos;
using Application.Customers.Queries;
using Application.Exceptions;
using AutoMapper;
using Infrastructure.Repositories.CustomerRepository;
using MediatR;


namespace Application.Customers.Handlers
{

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                // Handle case where customer is not found
                throw new NotFoundException("Customer not found");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }
    }

}

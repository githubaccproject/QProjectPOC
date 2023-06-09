﻿using Application.Customers.Commands;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.CustomerRepository;
using MediatR;

namespace Application.Customers.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }
            else {
                 _customerRepository.Detached(customer);
            }
            var UpdateCustomer = _mapper.Map<Customer>(request.Customer);
            _customerRepository.Update(UpdateCustomer);
            await _customerRepository.SaveChangesAsync();

            return customer.Id;
        }
    }
}

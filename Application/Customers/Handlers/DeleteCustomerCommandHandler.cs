using Application.Customers.Commands;
using Infrastructure.Repositories.CustomerRepository;
using MediatR;

namespace Application.Customers.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }
            _customerRepository.Delete(customer);
            await _customerRepository.SaveChangesAsync();

            return true;
        }
    }
}

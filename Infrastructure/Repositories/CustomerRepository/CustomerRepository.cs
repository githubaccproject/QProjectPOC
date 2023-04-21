using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories.CustomerRepository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDBContext dbContext) : base(dbContext)
        {

        }

    }
}

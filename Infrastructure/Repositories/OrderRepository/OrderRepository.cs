using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories.OrderRepository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }

}

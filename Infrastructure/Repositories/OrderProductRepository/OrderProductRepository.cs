using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories.OrderProductRepository
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }

}

using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }

}

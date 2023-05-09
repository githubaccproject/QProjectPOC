using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories.MembershipRepository
{
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        public MembershipRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }

}

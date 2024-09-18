using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FALOFinancialProofingDbContext dbContext) : base(dbContext)
        {
        }
    }
}

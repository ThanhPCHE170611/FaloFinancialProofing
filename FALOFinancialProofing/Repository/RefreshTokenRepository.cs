using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken, Guid>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FALOFinancialProofingDbContext dbContext) : base(dbContext)
        {
        }
    }
}
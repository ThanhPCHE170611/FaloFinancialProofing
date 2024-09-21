using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : DbContext
    {
        #region DBSet

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        #endregion DBSet

        public FALOFinancialProofingDbContext(DbContextOptions<FALOFinancialProofingDbContext> options) : base(options)
        {
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : DbContext
    {
        #region DBSet
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; } 
        #endregion
        public FALOFinancialProofingDbContext(DbContextOptions<FALOFinancialProofingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(UR => new { UR.Id });
                //entity.HasKey(UR => new { UR.UserId, UR.RoleId });
                entity.HasOne<User>(UR => UR.User)
                  .WithMany(U => U.UserRoles)
                  .HasForeignKey(s => s.UserId);
                entity.HasOne<Role>(UR => UR.Role)
                  .WithMany(R => R.UserRoles)
                  .HasForeignKey(UR => UR.RoleId);

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(U => U.Email).IsUnique();
            });
        }
    }
}

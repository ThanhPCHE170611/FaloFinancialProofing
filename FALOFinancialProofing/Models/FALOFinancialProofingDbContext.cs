using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : IdentityDbContext<User>
    {
        //#region DBSet

        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SDG> SDGs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }

        //#endregion DBSet

        public FALOFinancialProofingDbContext(DbContextOptions<FALOFinancialProofingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SDG>()
                .HasOne(s => s.User)
                .WithMany(u => u.SDGs)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<SocialNetwork>()
                .HasOne(s => s.User)
                .WithMany(u => u.SocialNetworks)
                .HasForeignKey(s => s.UserId);



            modelBuilder.Entity<CampaignMember>()
                .HasKey(cm => new { cm.Id, cm.UserId });

            modelBuilder.Entity<CampaignMember>()
                .HasOne(cm => cm.Campaign)
                .WithMany(c => c.CampaignMembers)
                .HasForeignKey(cm => cm.Id);

            modelBuilder.Entity<CampaignMember>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.CampaignMembers)
                .HasForeignKey(cm => cm.UserId);
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AppRole.Admin, NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AppRole.User, NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = AppRole.HR, NormalizedName = "Human Resources", ConcurrencyStamp = Guid.NewGuid().ToString() }
            );
        }
    }
}
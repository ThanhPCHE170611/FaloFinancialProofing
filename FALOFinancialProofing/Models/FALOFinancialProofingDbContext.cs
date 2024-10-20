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

        public DbSet<TransactionLog> TransactionLogs { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Campaign> Campaigns { get; set; } 
        public DbSet<CampaignMember> CampaignMembers { get; set; } 
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

            //modelBuilder.Entity<CampaignMember>()
            //.HasKey(cm => new { cm.CampaignId, cm.UserId });

            modelBuilder.Entity<CampaignMember>()
                .HasOne(cm => cm.Campaign)
                .WithMany(c => c.CampaignMembers)
                .HasForeignKey(cm => cm.CampaignId);

            modelBuilder.Entity<CampaignMember>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.CampaignMembers)
                .HasForeignKey(cm => cm.UserId);

            // Liên kết CreateBy với UserId trong bảng User
            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.User)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(c => c.CreateBy)
                .OnDelete(DeleteBehavior.Restrict); // Ngăn xóa User khi có liên kết
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "205d4496-4ac8-40d9-84b9-e09e1ada7a49", Name = AppRole.Admin, NormalizedName = "ADMIN", ConcurrencyStamp = "acccef8b-20f3-4de0-8ee9-5a3690f094ed" },
                new IdentityRole { Id = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", Name = AppRole.User, NormalizedName = "USER", ConcurrencyStamp = "1a777fbf-24db-4247-bd76-db376d703ea9" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2293", Name = AppRole.HR, NormalizedName = "Human Resources", ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85f" }
            );
        }
    }
}
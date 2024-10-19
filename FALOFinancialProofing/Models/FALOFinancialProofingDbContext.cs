using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : IdentityDbContext<User>
    {
        #region DBSet

        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<CreateProjectFile> CreateProjectFiles { get; set; }
        public DbSet<CreateProjectRequest> CreateProjects { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<SDG> SDGs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }

        #endregion DBSet

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

            modelBuilder.Entity<CampaignMember>(entity =>
            {
                entity.HasOne(c => c.Campaign)
                    .WithMany(u => u.CampaignMembers)
                    .HasForeignKey(c => c.CampaignID)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.User)
                   .WithMany(u => u.CampaignMembers)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CreateProjectRequest>(entity =>
            {
                entity.HasOne(c => c.SenderUser)
                    .WithMany(u => u.SenderCreateProjectRequests)
                    .HasForeignKey(c => c.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.ReceiverUser)
                   .WithMany(u => u.ReceiverCreateProjectRequests)
                   .HasForeignKey(c => c.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CreateProjectFile>(entity =>
            {
                entity.HasOne(c => c.ProjectRequest)
                    .WithMany(u => u.CreateProjectFiles)
                    .HasForeignKey(c => c.RequestId);

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(c => c.TransactionLogs)
                    .WithOne(u => u.SenderUser)
                    .HasForeignKey(c => c.SenderID);

            });


            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "205d4496-4ac8-40d9-84b9-e09e1ada7a49", Name = AppRole.Admin, NormalizedName = "ADMIN", ConcurrencyStamp = "acccef8b-20f3-4de0-8ee9-5a3690f094ed" },
                new IdentityRole { Id = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", Name = AppRole.User, NormalizedName = "USER", ConcurrencyStamp = "1a777fbf-24db-4247-bd76-db376d703ea9" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2293", Name = AppRole.HR, NormalizedName = "Human Resources", ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85f" }
            );
        }
    }
}
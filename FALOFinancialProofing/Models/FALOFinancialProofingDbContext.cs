using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : IdentityDbContext<User>
    {
        #region DBSet

        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<CreateProjectFile> CreateProjectFiles { get; set; }
        public DbSet<CreateProjectRequest> CreateProjects { get; set; }
        public DbSet<CreateCampaignFile> CreateCampaignFiles { get; set; }
        public DbSet<CreateCampaignRequest> CreateCampaignRequests { get; set; }
        public DbSet<MoveNextCampaignStatusRequest> MoveNextCampaignStatusRequests { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignMember> CampaignMembers { get; set; }
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
                    .HasForeignKey(c => c.CampaignId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.User)
                   .WithMany(u => u.CampaignMembers)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<MoveNextCampaignStatusRequest>(entity =>
            {
                entity.HasOne(c => c.Campaign)
                    .WithMany(u => u.MoveNextCampaignStatusRequests)
                    .HasForeignKey(c => c.CampaignID)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.SenderUser)
                   .WithMany(u => u.MoveNextCampaignStatusRequestSenderUsers)
                   .HasForeignKey(c => c.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.ReceiverUser)
                  .WithMany(u => u.MoveNextCampaignStatusRequestReceiverUsers)
                  .HasForeignKey(c => c.ReceiverId)
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
                entity.HasOne(c => c.Project)
                  .WithMany(u => u.CreateProjectRequests)
                  .HasForeignKey(c => c.ProjectId)
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
                entity.HasMany(c => c.Projects)
                   .WithOne(u => u.User)
                   .HasForeignKey(c => c.CreatedBy);
                entity.HasMany(c => c.Campaigns)
                  .WithOne(u => u.User)
                  .HasForeignKey(c => c.CreateBy)
                  .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasMany(c => c.Campaigns)
                    .WithOne(u => u.Project)
                    .HasForeignKey(c => c.ProjectId);

            });
            modelBuilder.Entity<CreateCampaignRequest>(entity =>
            {
                entity.HasMany(c => c.CreateCampaignFiles)
                    .WithOne(u => u.CampaignRequest)
                    .HasForeignKey(c => c.RequestId);
                entity.HasOne(c => c.Campaign)
                   .WithMany(u => u.CreateCampaignRequests)
                   .HasForeignKey(c => c.CampaignId);

                entity.HasOne(c => c.SenderUser)
                  .WithMany(u => u.CreateCampaignRequestSenders)
                  .HasForeignKey(c => c.SenderId)
                  .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.ReceiverUser)
                 .WithMany(u => u.CreateCampaignRequestReceivers)
                 .HasForeignKey(c => c.ReceiverId)
                 .OnDelete(DeleteBehavior.NoAction); ;
            });
            RoleSeedData(modelBuilder);
            DeleteIdentityPrefix(modelBuilder);
        }

        #region ConfigData
        public void RoleSeedData(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "205d4496-4ac8-40d9-84b9-e09e1ada7a49", Name = AppRole.Admin, NormalizedName = "ADMIN", ConcurrencyStamp = "acccef8b-20f3-4de0-8ee9-5a3690f094ed" },
                new IdentityRole { Id = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", Name = AppRole.User, NormalizedName = "USER", ConcurrencyStamp = "1a777fbf-24db-4247-bd76-db376d703ea9" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2293", Name = AppRole.HR, NormalizedName = "Human Resources", ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85f" }
            );
        }

        public void DeleteIdentityPrefix(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
        #endregion
    }
}
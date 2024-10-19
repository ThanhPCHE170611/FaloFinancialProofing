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
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SDG> SDGs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }


        public DbSet<RequestForm> RequestForms { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<ApproveProcess> ApproveProcesses { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        #endregion

        public FALOFinancialProofingDbContext(DbContextOptions<FALOFinancialProofingDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AttachmentFile>()
                .HasOne(a => a.RequestForm)
                .WithMany(r => r.AttachmentFiles)
                .HasForeignKey(a => a.RequestId);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.ApproveProcess)
                .WithMany(ap => ap.Vouchers)
                .HasForeignKey(v => v.ApproveId);
            
            modelBuilder.Entity<ApproveProcess>()
                .HasOne(ap => ap.RequestForm)
                .WithMany(r => r.ApproveProcesses)
                .HasForeignKey(ap => ap.RequestId);

            modelBuilder.Entity<ApproveProcess>()
                .HasOne(ap => ap.User)
                .WithMany(u => u.ApproveProcesses)
                .HasForeignKey(ap => ap.ApproverId);
            
            modelBuilder.Entity<ApproveProcess>()
                .HasOne(ap => ap.RequestForm)
                .WithMany(r => r.ApproveProcesses)
                .HasForeignKey(ap => ap.RequestId);
            
            modelBuilder.Entity<RequestForm>()
                .HasOne(r => r.User)
                .WithMany(u => u.RequestForms)
                .HasForeignKey(r => r.CreatedBy);
            
            modelBuilder.Entity<RequestForm>()
                .HasOne(r => r.Campaign)
                .WithMany(c => c.RequestForms)
                .HasForeignKey(r => r.CampaignId);

            modelBuilder.Entity<RequestForm>()
                .HasOne(r => r.RequestType)
                .WithMany(rt => rt.RequestForms)
                .HasForeignKey(r => r.TypeId);

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
                new IdentityRole { Id = "205d4496-4ac8-40d9-84b9-e09e1ada7a49", Name = AppRole.Admin, NormalizedName = "ADMIN", ConcurrencyStamp = "acccef8b-20f3-4de0-8ee9-5a3690f094ed" },
                new IdentityRole { Id = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", Name = AppRole.User, NormalizedName = "USER", ConcurrencyStamp = "1a777fbf-24db-4247-bd76-db376d703ea9" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2293", Name = AppRole.HR, NormalizedName = "Human Resources", ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85f" }
            );
        }
    }
}
using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System.Reflection.Emit;

namespace FALOFinancialProofing.Models
{
    public class FALOFinancialProofingDbContext : IdentityDbContext<User>
    {
        #region DBSet

        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<CreateProjectFile> CreateProjectFiles { get; set; }
        public DbSet<CreateProjectRequest> CreateProjectRequests { get; set; }
        public DbSet<CreateCampaignFile> CreateCampaignFiles { get; set; }
        public DbSet<CreateCampaignRequest> CreateCampaignRequests { get; set; }
        public DbSet<MoveNextCampaignStatusRequest> MoveNextCampaignStatusRequests { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignMember> CampaignMembers { get; set; }
        public DbSet<SDG> SDGs { get; set; }

        public DbSet<AccountingBook> AccountingBooks { get; set; }
        public DbSet<UserSDG> UserSDGs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<CreateProjectRequestApproveHistory> CreateProjectRequestApproveHistories { get; set; }
        public DbSet<CampaignRequestApproveHistory> CampaignRequestApproveHistories { get; set; }


        public DbSet<RequestForm> RequestForms { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<ApproveProcess> ApproveProcesses { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<MoveNextCampaignStatusRequestHistory> MoveNextCampaignStatusRequestHistories { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CreateQrCode> CreateQrCodes { get; set; }


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
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RequestForm>()
                .HasOne(r => r.Campaign)
                .WithMany(c => c.RequestForms)
                .HasForeignKey(r => r.CampaignId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MoveNextCampaignStatusRequestHistory>(entity =>
            {
                entity.HasOne(m => m.MoveNextCampaignStatusRequest)
                    .WithMany(h => h.MoveNextCampaignStatusRequestHistories)
                    .HasForeignKey(c => c.MoveNextCampaignStatusRequestId);

                entity.HasOne(r => r.Receiver)
                 .WithMany(h => h.MoveNextCampaignStatusRequestHistories)
                 .HasForeignKey(c => c.ReceiverId);
            });

            modelBuilder.Entity<RequestForm>()
                .HasOne(r => r.RequestType)
                .WithMany(rt => rt.RequestForms)
                .HasForeignKey(r => r.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserSDG>()
                .HasKey(us => new { us.Id });

            modelBuilder.Entity<UserSDG>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSDGs)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSDG>()
                .HasOne(us => us.SDG)
                .WithMany(s => s.UserSDGs)
                .HasForeignKey(us => us.SDGId);

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
            modelBuilder.Entity<TransactionLog>(entity =>
            {
                entity.HasOne(c => c.Campaign)
                    .WithMany(u => u.TransactionLogs)
                    .HasForeignKey(c => c.CampaignId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.CreateQrCode)
                    .WithMany(u => u.TransactionLogs)
                    .HasForeignKey(c => c.CreateQrCodeId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasIndex(u => u.CassoTransactionId)
                      .IsUnique();
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
                entity.HasIndex(u => u.Email)
                      .IsUnique();
                //entity.HasMany(c => c.TransactionLogs)
                //    .WithOne(u => u.SenderUser)
                //    .HasForeignKey(c => c.SenderID);
                entity.HasMany(c => c.Projects)
                   .WithOne(u => u.User)
                   .HasForeignKey(c => c.CreatedBy);
                entity.HasMany(u => u.RequestForms)
                        .WithOne(r => r.User)
                        .HasForeignKey(u => u.CreatedBy);
                entity.HasMany(c => c.Campaigns)
                  .WithOne(u => u.User)
                  .HasForeignKey(c => c.CreateBy)
                  .OnDelete(DeleteBehavior.NoAction);
                //them moi
                entity.HasMany(c => c.MoveNextCampaignStatusRequestSenderUsers)
                    .WithOne(u => u.SenderUser)
                    .HasForeignKey(su => su.SenderId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(c => c.MoveNextCampaignStatusRequestReceiverUsers)
                    .WithOne(u => u.ReceiverUser)
                    .HasForeignKey(ru => ru.ReceiverId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(c => c.CampaignRequestApproveHistories)
                  .WithOne(u => u.Approver)
                  .HasForeignKey(c => c.ApproverId);

                entity.HasMany(c => c.UserRoles)
                  .WithOne()
                  .HasForeignKey(c => c.UserId);

                entity.HasMany(c => c.CreateQrCodes)
               .WithOne(u => u.User)
               .HasForeignKey(c => c.UserId)
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
                 .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(c => c.CampaignRequestApproveHistories)
               .WithOne(u => u.CreateCampaignRequest)
               .HasForeignKey(c => c.CampaignRequestId)
               .OnDelete(DeleteBehavior.Restrict);


            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasMany(c => c.Projects)
                    .WithOne(u => u.Organization)
                    .HasForeignKey(c => c.OrganizationId);

            });

            modelBuilder.Entity<OrganizationMember>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany(u => u.OrganizationMembers)
                    .HasForeignKey(c => c.UserId);
                entity.HasOne(c => c.Organization)
                  .WithMany(u => u.OrganizationMembers)
                  .HasForeignKey(c => c.OrganizationId);

            });

            modelBuilder.Entity<CreateProjectRequestApproveHistory>(entity =>
            {
                entity.HasOne(c => c.CreateProjectRequest)
                    .WithMany(u => u.CreateProjectRequestApproveHistories)
                    .HasForeignKey(c => c.CreateProjectRequestId);

                entity.HasOne(c => c.Approver)
                 .WithMany(u => u.CreateProjectRequestApproveHistories)
                 .HasForeignKey(c => c.ApproverId);


            });

            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.AccountingBook)
                .WithOne(u => u.Campaign)
                .HasForeignKey<AccountingBook>(c => c.CampaignId);

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasOne(c => c.Bank)
                    .WithMany(u => u.Campaigns)
                    .HasForeignKey(c => c.BankId);

            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(u => u.CassoAccountID).IsUnique();
                entity.HasIndex(u => u.AccountNumber).IsUnique();
            });

            RoleSeedData(modelBuilder);
            DeleteIdentityPrefix(modelBuilder);
        }

        #region ConfigData
        public void RoleSeedData(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "205d4496-4ac8-40d9-84b9-e09e1ada7a49", Name = AppRole.ProjectManager, NormalizedName = AppRole.ProjectManager.ToUpper(), ConcurrencyStamp = "acccef8b-20f3-4de0-8ee9-5a3690f094ed" },
                new IdentityRole { Id = "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", Name = AppRole.VolunteerLeader, NormalizedName = AppRole.VolunteerLeader.ToUpper(), ConcurrencyStamp = "1a777fbf-24db-4247-bd76-db376d703ea9" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2293", Name = AppRole.Accounting, NormalizedName = AppRole.Accounting.ToUpper(), ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85f" },
                new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2295", Name = AppRole.Volunteer, NormalizedName = AppRole.Volunteer.ToUpper(), ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85g" },
                 new IdentityRole { Id = "83292e2c-6c86-4153-bdc5-760d05ec2299", Name = AppRole.ProjectManagementBoard, NormalizedName = AppRole.ProjectManagementBoard.ToUpper(), ConcurrencyStamp = "606fea67-ae89-4b3f-ac93-ccceda6fc85h" },
                 new IdentityRole { Id = "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb", Name = AppRole.Admin, NormalizedName = AppRole.Admin.ToUpper(), ConcurrencyStamp = "ba58588f-f626-41a0-8fca-b74481367335" },
                 new IdentityRole { Id = "15db7f37-5dbc-4035-9b00-a0af4c3fe8bd", Name = AppRole.Donor, NormalizedName = AppRole.Donor.ToUpper(), ConcurrencyStamp = "ba58588f-f626-41a0-8fca-b74481367337" }
            );

            modelBuilder.Entity<RequestType>().HasData(
                new RequestType { Id = 1, TypeName = "Pre-Pay" },
                new RequestType { Id = 2, TypeName = "Payment" }
             );

            // Seed Bank
            modelBuilder.Entity<Bank>().HasData(
                new Bank { Id = 1, OwnerName = "Nguyen Van Duc", AccountNumber = "1016161976", BankCodeName = "VietComBank", acqId = 970436, CassoAccountID = 123 },
                 new Bank { Id = 2, OwnerName = "Bui Minh Manh", AccountNumber = "4270774590", BankCodeName = "BIDV", acqId = 970418, CassoAccountID = 222 }

            );
            // seed SDG
            modelBuilder.Entity<SDG>().HasData(
               new SDG() { Id = 1, SDGName = "No Poverty" }, new SDG() { Id = 2, SDGName = "Zero Hunger" }, new SDG() { Id = 3, SDGName = "Good Health And Well-Being" }, new SDG() { Id = 4, SDGName = "Quality Education" }, new SDG() { Id = 5, SDGName = "Gender Equality" }, new SDG() { Id = 6, SDGName = "Clean Water And Sanitation" }

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
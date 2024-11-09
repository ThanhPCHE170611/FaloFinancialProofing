using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FALOFinancialProofing.Models
{
    public class Campaign : Entity<int>
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Required]
        public string CreateBy { get; set; } // Đây là khóa ngoại trỏ tới UserId của User
        public virtual User User { get; set; } = null!;

        [MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public double FundTarget { get; set; }

        [MaxLength(200)]
        public string? Image { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [AllowNull]
        public string? UpdateLog { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public string? BankingNumber { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }

        public string? Status { get; set; }

        public ICollection<CampaignMember> CampaignMembers { get; set; } = new List<CampaignMember>();
        public ICollection<RequestForm> RequestForms { get; set; } = new List<RequestForm>();
        public ICollection<MoveNextCampaignStatusRequest> MoveNextCampaignStatusRequests { get; set; } = new List<MoveNextCampaignStatusRequest>();
        public ICollection<CreateCampaignRequest> CreateCampaignRequests { get; set; } = new List<CreateCampaignRequest>();
        public ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();

        public AccountingBook? AccountingBook { get; set; }

    }
}
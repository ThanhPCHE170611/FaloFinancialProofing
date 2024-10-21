using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace FALOFinancialProofing.Models
{
    public class RequestForm : Entity<int>
    {
        public DateTime CreateAt { get; set; }
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }
        
        public string Status { get; set; }

        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        public int CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }

        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual RequestType RequestType { get; set; }
        public ICollection<AttachmentFile> AttachmentFiles { get; set; } = new List<AttachmentFile>();
        public ICollection<ApproveProcess> ApproveProcesses { get; set; } = new List<ApproveProcess>();
    }
}

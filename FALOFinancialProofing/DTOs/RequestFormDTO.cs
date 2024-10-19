using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.DTOs
{
    public class RequestFormDTO
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }

        public string Status { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public int CampaignId { get; set; }
        [Required]
        public int TypeId { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; } = new List<AttachmentFile>();
        public List<ApproveProcess> ApproveProcesses { get; set; } = new List<ApproveProcess>();
    }
}

using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs
{
    public class RequestFormWithAttachmentApprovementVoucher
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }

        public string? CreateByName { get; set; }

        public string? CreateByEmail { get; set; }

        public string? FeedBack { get; set;}

        public int CampaignId { get; set; }
        public int TypeId { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; } = new List<AttachmentFile>();
        public List<ApproveProcessWithUser> ApproveProcesses { get; set; } = new List<ApproveProcessWithUser>();
        public List<VoucherRequest> VoucherFiles { get; set; } = new List<VoucherRequest>();
    }
}

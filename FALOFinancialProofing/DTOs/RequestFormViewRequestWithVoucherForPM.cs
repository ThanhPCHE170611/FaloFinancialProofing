using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs
{
    public class RequestFormViewRequestWithVoucherForPM
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }

        public string? Status { get; set; }
        public string CreatedBy { get; set; }
        public int CampaignId { get; set; }

        public string? ApproveProcessStatus { get; set; }
        public List<AttachmentFileRequest> AttachmentFiles { get; set; }

        public List<Voucher>? VoucherFiles { get; set; }
    }
}

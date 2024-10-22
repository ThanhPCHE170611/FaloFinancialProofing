using Humanizer.Localisation;

namespace FALOFinancialProofing.DTOs
{
    public class PrePayRequestFormViewRequest
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; } 
        public string Description { get; set; }

        public double ExpectedMoney { get; set; }

        public string? Status { get; set; } 
        public string CreatedBy { get; set; }
        public int CampaignId { get; set; }

        public List<AttachmentFileRequest> AttachmentFiles { get; set; }

    }
}

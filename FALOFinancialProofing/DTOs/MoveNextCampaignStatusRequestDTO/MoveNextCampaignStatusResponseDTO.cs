using System.ComponentModel;

namespace FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO
{
    public class MoveNextCampaignStatusResponseDTO
    {
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public int CampaignID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        [DefaultValue("Pending")]
        public string Status { get; set; }
        public string StatusOfCampaign { get; set; }
    }
}

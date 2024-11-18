using FALOFinancialProofing.DTOs.CreateCampaignFileDTO;
using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CreateCampaignRequestDTO
{
    public class CreateCampaignRequestInformation
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int CampaignId { get; set; }
        public string? Title { get; set; }
        public string? ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        public double? FundTarget { get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Address { get; set; }
        public ICollection<CreateCampaignFileInformation> CreateCampaignFiles { get; set; }
    }
}

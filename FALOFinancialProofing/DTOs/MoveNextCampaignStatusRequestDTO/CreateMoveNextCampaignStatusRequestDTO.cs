using FALOFinancialProofing.Core;
using FALOFinancialProofing.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO
{
    public class CreateMoveNextCampaignStatusRequestDTO 
    {
      
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public int CampaignID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusOfCampaign { get; set; }
    }
}

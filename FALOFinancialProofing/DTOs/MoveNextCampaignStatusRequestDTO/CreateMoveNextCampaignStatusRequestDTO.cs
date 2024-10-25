using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO
{
    public class CreateMoveNextCampaignStatusRequestDTO
    {
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public int CampaignID { get; set; }
        public string Title { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
    }
}

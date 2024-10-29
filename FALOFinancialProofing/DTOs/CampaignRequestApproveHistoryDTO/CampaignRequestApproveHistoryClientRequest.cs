using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CampaignRequestApproveHistoryDTO
{
    public class CampaignRequestApproveHistoryClientRequest
    {
        public int CampaignRequestId { get; set; }
        public string ApproverId { get; set; }
        public DateTime DateOfApproval { get; set; }
        [Required]
        public bool IsAllowed { get; set; }
        public string? BankingNumber { get; set; }
    }
}

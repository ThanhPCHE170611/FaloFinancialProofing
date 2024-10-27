using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.CampaignRequestApproveHistoryDTO
{
    public class CampaignRequestApproveHistoryClientRequest
    {
        public int CampaignRequestId { get; set; }
        public string ApproverId { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
    }
}

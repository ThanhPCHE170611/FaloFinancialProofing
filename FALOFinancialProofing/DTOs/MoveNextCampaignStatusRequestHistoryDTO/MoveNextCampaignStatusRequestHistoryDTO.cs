using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestHistoryDTO
{
    public class MoveNextCampaignStatusRequestHistoryDTO
    {
        public int MoveNextCampaignStatusRequestId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
    }
}

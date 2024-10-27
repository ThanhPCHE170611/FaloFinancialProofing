using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class MoveNextCampaignStatusRequestHistory : Entity<int>
    {
        public int MoveNextCampaignStatusRequestId { get; set; }
        public MoveNextCampaignStatusRequest MoveNextCampaignStatusRequest { get; set; }
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
    }
}

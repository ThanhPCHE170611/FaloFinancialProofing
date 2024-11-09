using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class CampaignRequestApproveHistory : Entity<int>
    {
        public int CampaignRequestId { get; set; }
        public CreateCampaignRequest CreateCampaignRequest { get; set; }
        public string ApproverId { get; set; }
        public User Approver { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
        public string? FeedBack { get; set; }
    }
}

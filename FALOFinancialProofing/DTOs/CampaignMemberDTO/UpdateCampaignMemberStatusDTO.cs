using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class UpdateCampaignMemberStatusDTO : Entity<int>
    {
        public string PmUserId { get; set; } // pm, pmb, admin
        public string PmRoleId { get; set; } // của pm (k update)
        public int CampaignId { get; set; } // pm, pmb, admin
        public bool IsActive { get; set; }
    }
}

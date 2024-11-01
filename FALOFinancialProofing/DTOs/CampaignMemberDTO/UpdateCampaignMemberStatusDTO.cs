using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class UpdateCampaignMemberStatusDTO : Entity<int>
    {
        public bool IsActive { get; set; }
    }
}

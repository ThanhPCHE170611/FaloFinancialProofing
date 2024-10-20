using System.ComponentModel;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class UpdateCampaignMemberDTO
    {
        public int CampaignId { get; set; }  
        public string UserId { get; set; }  
        public double Debt { get; set; }
        public bool IsActive { get; set; }
    }
}

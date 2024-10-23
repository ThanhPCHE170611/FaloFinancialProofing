using System.ComponentModel;
using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class UpdateCampaignMemberDTO : Entity<int>
    {
        //public int CampaignId { get; set; }  
        //public string UserId { get; set; }  
        public double Debt { get; set; }
        public bool IsActive { get; set; }
    }
}

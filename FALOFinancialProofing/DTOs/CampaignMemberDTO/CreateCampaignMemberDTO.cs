using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class CreateCampaignMemberDTO
    {
        public int CampaignId { get; set; }

        public string UserId { get; set; } = null!;

    }
}

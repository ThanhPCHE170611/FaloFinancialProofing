using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CampaignMemberDTO
{
    public class CreateManyCampaignMemberDTO
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string RoleId { get; set; } = null!;
    }
}

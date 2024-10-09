using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CampaignMember : Entity<int>
    {
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; } = null!;

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}

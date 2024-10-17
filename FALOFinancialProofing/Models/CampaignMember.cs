using FALOFinancialProofing.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CampaignMember : Entity<int>
    {
        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        [Column(TypeName = "money")]
        [DefaultValue(0)]
        public decimal Debt { get; set; } = 0;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}

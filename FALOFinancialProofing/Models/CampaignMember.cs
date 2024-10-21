using FALOFinancialProofing.Core;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CampaignMember : Entity<int>
    {
        //[ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        [Column(TypeName = "money")]
        [DefaultValue(0)]
        public double Debt { get; set; } = 0;

        [ForeignKey(nameof(IdentityRole))]
        public string RoleId { get; set; } = null!;
        public virtual IdentityRole IdentityRole { get; set; } = null!;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}

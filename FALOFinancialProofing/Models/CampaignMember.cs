using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CampaignMember : Entity<int>
    {

        public string CampaignID { get; set; }
        public Campaign Campaign { get; set; } = null!;
        public string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

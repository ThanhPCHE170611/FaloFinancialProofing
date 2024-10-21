using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CampaignMember : Entity<int>
    {

        public int CampaignID { get; set; }
        public Campaign Campaign { get; set; } = null!;
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public Double Debt { get; set; }
        public bool IsActive { get; set; }
    }
}

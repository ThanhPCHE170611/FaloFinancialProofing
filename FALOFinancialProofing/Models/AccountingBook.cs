using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class AccountingBook : Entity<int>
    {
        public string FilePath { get; set; }

        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }
    }
}

using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class CreateCampaignFile : Entity<int>
    {
        public int RequestId { get; set; }
        public string FilePath { get; set; }
        public CreateCampaignRequest CampaignRequest { get; set; }
    }
}

namespace FALOFinancialProofing.DTOs
{
    public class UserWithDeb
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserRole { get; set; }

        public bool IsActive { get; set; }

        public double Debt { get; set; }

        public int CampaignId { get; set; }

        public string CampaignName { get; set; }
    }
}

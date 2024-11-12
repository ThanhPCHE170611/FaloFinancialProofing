namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class UserTransactionHistory
    {
        public string UserId { get; set; }
        public int CreateQrCodeId { get; set; }
        public bool IsPaid { get; set; }
        public int Amount { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string tid { get; set; }

    }
}

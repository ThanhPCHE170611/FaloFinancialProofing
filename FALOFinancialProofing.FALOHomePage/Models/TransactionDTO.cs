namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class TransactionDTO
    {
        public class TransactionLog
        {
            public string UserId { get; set; }
            public int CreateQrCodeId { get; set; }
            public bool IsPaid { get; set; }
            public decimal Amount { get; set; }
            public int CampaignId { get; set; }
            public string CampaignName { get; set; }
            public string Description { get; set; }
            public DateTime TransactionDate { get; set; }
            public string TId { get; set; }
        }

        public class TransactionLogResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public TransactionLogData Data { get; set; }
        }

        public class TransactionLogData
        {
            public List<TransactionLog> Data { get; set; }
            public int CurrentPage { get; set; }
            public int DataCount { get; set; }
        }
    }
}

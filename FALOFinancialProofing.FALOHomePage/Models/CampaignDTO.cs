namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class CampaignDTO
    {
        public int CampaignId { get; set; }
        public int ProjectId { get; set; }
        public string CreateBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public decimal FundTarget { get; set; }
        public decimal? TotalMoneyEarned { get; set; }
        public string? Image { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string BankingNumber { get; set; }
        public string Status { get; set; }
        public int BankId { get; set; }
    }

    public class CampaignData
    {
        public List<CampaignDTO> Data { get; set; } // List of campaigns
        public int CurrentPage { get; set; }
        public int DataCount { get; set; }
    }

    // Represents the entire response structure
    public class ApiResponseCampaign
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CampaignData? Data { get; set; }
    }
}

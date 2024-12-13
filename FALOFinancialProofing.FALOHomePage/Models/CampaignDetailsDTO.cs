namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class CampaignDetailsDTO
    {
        public int CampaignId { get; set; }
        public int ProjectId { get; set; }
        public string CreateBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public long FundTarget { get; set; }
        public long TotalMoneyEarned { get; set; }
        public string Image { get; set; }
        public DateTime? EndDate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public int BankId { get; set; }
        public string? UpdateLog { get; set; }
        public ICollection<CreateCampaignFileInformation> CreateCampaignFiles { get; set; } = new List<CreateCampaignFileInformation>();

    }
    public class ApiResponseCampaignDetails
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CampaignDetailsDTO Data { get; set; }
    }
}

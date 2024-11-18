namespace FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO
{
    public class MoveNextCampaignStatusRequestInformation
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusOfCampaign { get; set; }
    }
}

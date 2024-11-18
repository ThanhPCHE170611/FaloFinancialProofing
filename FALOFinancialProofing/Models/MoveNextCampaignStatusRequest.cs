using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class MoveNextCampaignStatusRequest : Entity<int>
    {
        public MoveNextCampaignStatusRequest()
        {
            MoveNextCampaignStatusRequestHistories = new List<MoveNextCampaignStatusRequestHistory>();
        }
        [Required]
        public string SenderId { get; set; }
        public User SenderUser { get; set; }
        public string? ReceiverId { get; set; }
        public User ReceiverUser { get; set; }
        [Required]
        public int CampaignID { get; set; }
        public Campaign Campaign { get; set; }
        public string Title { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [StringLength(250)]
        public string? Feedback { get; set; }
        public string Description { get; set; }
        public string StatusOfCampaign { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public ICollection<MoveNextCampaignStatusRequestHistory> MoveNextCampaignStatusRequestHistories { get; set; }
    }
}

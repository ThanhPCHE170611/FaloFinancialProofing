using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    // chưa liên kết khóa ngoại với bảng user, campainmember
    public class MoveNextCampaignStatusRequest : Entity<int>
    {
        // chưa rõ ràng senderId và receiverId là gì sao lại nối vào bảng campaign member mà không có khóa khóa ngoại
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int CampaignID { get; set; }
        public Campaign Campaign { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(250)]
        public string Feedback { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
    }
}

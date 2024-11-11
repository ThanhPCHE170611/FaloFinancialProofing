using FALOFinancialProofing.Core;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Models
{
    public class CreateProjectRequestApproveHistory : Entity<int>
    {
        public int CreateProjectRequestId { get; set; }
        [JsonIgnore]
        public CreateProjectRequest CreateProjectRequest { get; set; }
        public string ApproverId { get; set; }
        [JsonIgnore]
        public User Approver { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
        public string? FeedBack { get; set; }
    }
}

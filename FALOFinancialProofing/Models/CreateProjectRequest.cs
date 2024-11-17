using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class CreateProjectRequest : Entity<int>
    {
        public CreateProjectRequest()
        {
            CreateProjectFiles = new List<CreateProjectFile>();
            CreateProjectRequestApproveHistories = new List<CreateProjectRequestApproveHistory>();
        }
        public string SenderId { get; set; }
        public User SenderUser { get; set; }
        public string? ReceiverId { get; set; }
        public User ReceiverUser { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string? Title { get; set; }
        [Required(ErrorMessage = "CreatedAt is Required")]
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        [StringLength(20, MinimumLength = 0, ErrorMessage = "String Length Must be in 0 to 20")]
        public string Status { get; set; }
        public string Description { get; set; }
        public ICollection<CreateProjectFile> CreateProjectFiles { get; set; }
        public ICollection<CreateProjectRequestApproveHistory> CreateProjectRequestApproveHistories { get; set; }
    }
}

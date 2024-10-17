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
        }
        public string SenderId { get; set; }
        public User SenderUser { get; set; }
        public string ReceiverId { get; set; }
        public User ReceiverUser { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public DateTime CreatedAt { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "String Length Must be in 0 to 20")]
        public DateTime Feedback { get; set; }
        [StringLength(20, MinimumLength = 0, ErrorMessage = "String Length Must be in 0 to 20")]
        public string Status { get; set; }
        public ICollection<CreateProjectFile> CreateProjectFiles { get; set; }
    }
}

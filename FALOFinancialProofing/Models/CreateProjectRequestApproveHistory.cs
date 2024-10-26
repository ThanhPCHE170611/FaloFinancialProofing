using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class CreateProjectRequestApproveHistory : Entity<int>
    {
        public int CreateProjectRequestId { get; set; }
        public CreateProjectRequest CreateProjectRequest { get; set; }
        public string ApproverId { get; set; }
        public User Approver { get; set; }
        public DateTime DateOfApproval { get; set; }
    }
}

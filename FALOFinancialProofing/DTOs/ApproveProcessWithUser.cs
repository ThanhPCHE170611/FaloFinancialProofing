using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs
{
    public class ApproveProcessWithUser
    {
        public int? Id { get; set; }
        public int ApproveNumber { get; set; }

        public string? ApproveStatus { get; set; }

        public int RequestId { get; set; }

        public string ApproverId { get; set; }
        public UserWithRole UserWithRole { get; set; }
    }
}

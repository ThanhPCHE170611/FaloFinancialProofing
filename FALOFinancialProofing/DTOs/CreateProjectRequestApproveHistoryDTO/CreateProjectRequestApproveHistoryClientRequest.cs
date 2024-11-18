using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.CreateProjectRequestApproveHistoryDTO
{
    public class CreateProjectRequestApproveHistoryClientRequest
    {
        //public int ProjectId { get; set; }
        public int CreateProjectRequestId { get; set; }
        public string ApproverId { get; set; }
        public DateTime DateOfApproval { get; set; }
        public bool IsAllowed { get; set; }
        public string? FeedBack { get; set; }
    }
}

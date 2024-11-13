using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.CreateProjectRequestDTO
{
    public class CreateProjectRequestInformation
    {
        //CreateProjectRequest
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Feedback { get; set; }
        public string Status { get; set; }
        public string? ProjectDescription { get; set; }
        public ICollection<CreateProjectFileInformation> CreateProjectFiles { get; set; }
        //public ICollection<CreateProjectRequestApproveHistory> CreateProjectRequestApproveHistories { get; set; }

    }
}

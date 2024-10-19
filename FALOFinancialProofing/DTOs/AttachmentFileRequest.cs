using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FALOFinancialProofing.DTOs
{
    public class AttachmentFileRequest
    {
        public int? Id { get; set; }
        public string? FilePath { get; set; }
        [Required]
        public int RequestId { get; set; }
    }
}

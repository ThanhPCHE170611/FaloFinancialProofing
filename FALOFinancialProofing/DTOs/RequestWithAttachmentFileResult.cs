using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs
{
    public class RequestWithAttachmentFileResult
    {
        public int? Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Description { get; set; }

        public double? ExpectedMoney { get; set; }

        public string? CreateByName { get; set; }

        public string? CreateByEmail { get; set; }
        public string? AttachmentFilePath { get; set; }
    }
}

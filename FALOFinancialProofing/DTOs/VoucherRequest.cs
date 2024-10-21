using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class VoucherRequest
    {
        public int? Id { get; set; }
        public string? FilePath { get; set; }

        public string? Status { get; set; }

        [Required]
        public int ApproveId { get; set; }
    }
}

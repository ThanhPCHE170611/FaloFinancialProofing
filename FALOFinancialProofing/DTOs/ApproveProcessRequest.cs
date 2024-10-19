using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.DTOs
{
    public class ApproveProcessRequest
    {
        public int? Id { get; set; }
        public int ApproveNumber { get; set; }

        public string? ApproveStatus { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public string ApproverId { get; set; }
        public List<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}

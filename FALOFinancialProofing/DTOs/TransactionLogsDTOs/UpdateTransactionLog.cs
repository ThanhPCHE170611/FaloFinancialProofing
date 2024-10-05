using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.TransactionLogsDTOs
{
    public class UpdateTransactionLog : Entity<int>
    {
        [Required(ErrorMessage ="Sender is require!")]
        public string SenderID { get; set; }
        [Required(ErrorMessage = "Amount is require!")]
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "ReceiverId is require!")]
        public string ReceiverId { get; set; }
    }
}

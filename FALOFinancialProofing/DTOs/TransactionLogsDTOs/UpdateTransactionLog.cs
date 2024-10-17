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
        public string BankId { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "ReceiverId is require!")]
        public int CampaignId { get; set; }
    }
}

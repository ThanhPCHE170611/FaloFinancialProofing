using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.TransactionLogsDTOs
{
    public class CreateTransactionLog
    {
        [Required]
        public string SenderID { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string BankId { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Required]
        public int CampaignId { get; set; }
    }
}

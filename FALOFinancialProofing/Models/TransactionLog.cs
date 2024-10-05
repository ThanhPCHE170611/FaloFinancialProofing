using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class TransactionLog : Entity<int>
    {
        public string SenderID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ReceiverId { get; set; }
    }
}

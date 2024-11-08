using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class CreateQrCode : Entity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int Amount { get; set; }
        public bool IsPaid { get; set; }
        public List<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
    }
}

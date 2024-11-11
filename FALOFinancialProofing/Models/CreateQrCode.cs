using FALOFinancialProofing.Core;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Models
{
    public class CreateQrCode : Entity<int>
    {
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int Amount { get; set; }
        public bool IsPaid { get; set; }
        [JsonIgnore]
        public List<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
    }
}

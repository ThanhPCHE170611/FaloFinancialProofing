using FALOFinancialProofing.Core;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Models
{
    public class TransactionLog : Entity<int>
    {

        //[JsonPropertyOrder(3)]
        //public string ReceiverId { get; set; }
        [JsonPropertyOrder(2)]
        public string SenderID { get; set; }
        [JsonPropertyOrder(3)]
        public string BankId { get; set; }
        [JsonPropertyOrder(4)]
        public decimal Amount { get; set; }
        [JsonPropertyOrder(5)]
        public int CampaignId { get; set; }
        [JsonPropertyOrder(6)]
        public string Description { get; set; }
        [JsonPropertyOrder(7)]
        public DateTime TransactionDate { get; set; }

    }
}

using FALOFinancialProofing.Core;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Models
{
    public class TransactionLog : Entity<int>
    {

        //[JsonPropertyOrder(3)]
        //public string ReceiverId { get; set; }
        //[JsonPropertyOrder(2)]
        //public string SenderID { get; set; }
        //public User SenderUser { get; set; }
        [JsonPropertyOrder(2)]
        public string BankId { get; set; }
        [JsonPropertyOrder(3)]
        public double Amount { get; set; }
        [JsonPropertyOrder(4)]
        public int CampaignId { get; set; } 
        public Campaign Campaign { get; set; } 
        [JsonPropertyOrder(5)]
        public string Description { get; set; }
        [JsonPropertyOrder(6)]
        public DateTime TransactionDate { get; set; }
       

    }
}

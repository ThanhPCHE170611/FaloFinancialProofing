using FALOFinancialProofing.Core;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.Models
{
    public class TransactionLog : Entity<int>
    {
        [JsonPropertyOrder(2)]
        public int? CreateQrCodeId { get; set; }
        public CreateQrCode CreateQrCode { get; set; }
        [JsonPropertyOrder(3)]
        public int Amount { get; set; }
        [JsonPropertyOrder(4)]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        [JsonPropertyOrder(5)]
        public string Description { get; set; }
        [JsonPropertyOrder(6)]
        public DateTime TransactionDate { get; set; }
        // mã giao dịch từ phía casso
        public long? CassoTransactionId { get; set; }
        // mã giao dịch từ phía ngân hàng
        public string tid { get; set; }



    }
}

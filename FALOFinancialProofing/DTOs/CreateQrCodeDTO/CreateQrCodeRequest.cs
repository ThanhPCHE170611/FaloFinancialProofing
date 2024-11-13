using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.CreateQrCodeDTO
{
    public class CreateQrCodeRequest
    {
        public string UserId { get; set; }
        public int Amount { get; set; }
        //public bool IsPaid { get; set; } = false;
        public int BankId { get; set; }
        public int CampaignId { get; set; }
    }
}

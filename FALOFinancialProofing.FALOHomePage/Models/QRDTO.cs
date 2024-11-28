namespace FALOFinancialProofing.FALOHomePage.Models
{
    public class QRDTO
    {
        public string UserId {  get; set; }
        public long Amount { get; set; }
        public int BankId { get; set; }
        public int CampaignId { get; set; }

        public QRDTO(string uId, long amount, int bId, int cId) { 
            UserId = uId;
            Amount = amount;
            BankId = bId;
            CampaignId = cId;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.BankDTO
{
    public class BankInformation
    {
        public int Id { get; set; }
        [Length(0, 100)]
        public string OwnerName { get; set; } // need
        [Length(6, 19)]
        public string AccountNumber { get; set; }// need
        [Length(0, 30)]
        public string BankCodeName { get; set; }
        // bankBin
        public int acqId { get; set; }// need
        public long CassoAccountID { get; set; }
    }
}

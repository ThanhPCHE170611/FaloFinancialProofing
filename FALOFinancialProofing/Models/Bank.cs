using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Models
{
    public class Bank : Entity<int>
    {
        [Length(0, 100)]
        public string OwnerName { get; set; }
        [Length(6, 19)]
        public string AccountNumber { get; set; }

        [Length(0, 30)]
        public string BankCodeName { get; set; }
        // bankBin
        public int acqId { get; set; }
        public long CassoAccountID { get; set; }
        //public bool IsInUsed { get; set; }

    }
}

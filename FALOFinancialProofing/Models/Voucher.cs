using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class Voucher : Entity<int>
    {
        public string FilePath { get; set; }   

        public string Status { get; set; }

        public int ApproveId { get; set; }
        [ForeignKey("ApproveId")]
        public virtual ApproveProcess ApproveProcess { get; set; }
    }
}

using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class ApproveProcess : Entity<int>
    {
        public int ApproveNumber { get; set; }

        public string ApproveStatus { get; set; }

        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual RequestForm RequestForm { get; set; }

        public string ApproverId { get; set; }
        [ForeignKey("ApproverId")]
        public virtual User User { get; set; }
        public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}

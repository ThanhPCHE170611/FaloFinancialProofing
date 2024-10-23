using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class AttachmentFile : Entity<int>
    {
        public string FilePath { get; set; }

        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual RequestForm RequestForm { get; set; }
    }
}

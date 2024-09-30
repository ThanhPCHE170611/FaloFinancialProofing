using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class SDG : Entity<int>
    {
        [ForeignKey("User")]
        public string SDGName { get; set; }

        public virtual User User { get; set; }
    }
}

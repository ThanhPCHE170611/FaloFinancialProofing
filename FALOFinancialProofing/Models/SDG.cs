using FALOFinancialProofing.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FALOFinancialProofing.Models
{
    public class SDG : Entity<int>
    {
        
        public string SDGName { get; set; }

        public ICollection<UserSDG> UserSDGs { get; set; } = new List<UserSDG>();
    }
}

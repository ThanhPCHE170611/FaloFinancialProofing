using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class UserSDG : Entity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int SDGId { get; set; }
        public SDG SDG { get; set; }
    }
}

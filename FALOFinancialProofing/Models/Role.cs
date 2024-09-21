using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class Role : Entity<int>
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
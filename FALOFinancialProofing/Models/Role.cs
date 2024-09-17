using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.Models
{
    public class Role : Entity
    {
        public Role()
        {
            
        }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

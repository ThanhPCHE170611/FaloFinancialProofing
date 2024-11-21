using Microsoft.AspNetCore.Identity;

namespace FALOFinancialProofing.Models
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

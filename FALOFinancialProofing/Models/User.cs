using FALOFinancialProofing.Core;
using Microsoft.AspNetCore.Identity;

namespace FALOFinancialProofing.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public virtual SDG SDG { get; set; } 
    //public DateOnly? BirthDate { get; set; }
    //public string? Gender { get; set; }
    //public string? Address { get; set; }
    //public string? PhoneNumber { get; set; }
    //public string Email { get; set; }
    //public string SocialAddress { get; set; }
    //public string? Avatar { get; set; }
    //public string Password { get; set; }
    //public int Status { get; set; }
    //public virtual List<Role> Roles { get; set; }
}
}
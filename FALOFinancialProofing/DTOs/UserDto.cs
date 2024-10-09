using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs
{
    public class UserDto : Entity<int>
    {
        public string Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateOnly? BirthDate { get; set; } 

        public List<String> RoleNames { get; set; }
    }
}
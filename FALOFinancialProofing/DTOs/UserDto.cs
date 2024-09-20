using FALOFinancialProofing.Core;

namespace FALOFinancialProofing.DTOs
{
    public class UserDto : Entity<int>
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }

        //public List<String> RoleNames { get; set; }
        public String? RoleName { get; set; }
    }
}
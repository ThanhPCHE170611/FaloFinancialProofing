using FALOFinancialProofing.DTOs.RoleDTOs;

namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class UserInformation
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public List<RoleInformation> Roles { get; set; } = new List<RoleInformation>();
    }
}

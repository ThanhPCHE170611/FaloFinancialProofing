using FALOFinancialProofing.DTOs.RoleDTOs;

namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class SelectedUserInformation
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public RoleInformation RoleInformation { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.UserDTOs
{
    public class SignUpAdminRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}

using FALOFinancialProofing.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs
{
    public class SignUpRequest
    {
        [MinLength(1), Required, NameValidate(ErrorMessage = "Name must not have digit or special character")]
        public string FirstName { get; set; }

        [MinLength(1), Required, NameValidate(ErrorMessage = "Name must not have digit or special character")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [UsernameValidate(ErrorMessage = "UserName must not have blank."), Required]
        public string UserName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SocialAddress { get; set; }
        public string? Avatar { get; set; }
        public int? Status { get; set; }
    }
}

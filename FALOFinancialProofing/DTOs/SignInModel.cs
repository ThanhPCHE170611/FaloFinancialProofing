using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FALOFinancialProofing.DTOs
{
    public class SignInModel
    {
        //[Required, EmailAddress]
        [Required]
        public string UserName { get; set; }
        //[Required]
        //[JsonIgnore]
        //public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

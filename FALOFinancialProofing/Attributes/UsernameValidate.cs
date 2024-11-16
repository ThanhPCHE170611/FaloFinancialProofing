using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FALOFinancialProofing.Attributes
{
    public class UsernameValidate : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(@"^\S+$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is string username && _regex.IsMatch(username))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "UserName must not have blank.");
        }
    }
}

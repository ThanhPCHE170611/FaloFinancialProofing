using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FALOFinancialProofing.Attributes
{
    public class NameValidate : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(@"^[a-zA-Z\s]+$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is string name && _regex.IsMatch(name))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "String must not have digit and speical character.");
        }
    }
}

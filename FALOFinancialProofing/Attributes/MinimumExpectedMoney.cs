using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Attributes
{
    public class MinimumExpectedMoney : ValidationAttribute
    {
        private readonly double _minimumExpectedMoney;

        public MinimumExpectedMoney(double minimumExpectedMoney)
        {
            _minimumExpectedMoney = minimumExpectedMoney;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is double doubleValue && doubleValue > _minimumExpectedMoney)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? $"Value must be greater than {_minimumExpectedMoney}.");
        }
    }
}

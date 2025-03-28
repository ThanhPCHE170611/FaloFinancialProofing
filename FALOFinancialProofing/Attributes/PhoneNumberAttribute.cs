using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace FALOFinancialProofing.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private readonly string _pattern;
        public PhoneNumberAttribute(string pattern = @"^[0-9]+$")
        {
            _pattern = pattern;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Hoặc bạn có thể trả về lỗi nếu số điện thoại là bắt buộc
            }

            string phoneNumber = value.ToString();
            if (Regex.IsMatch(phoneNumber, _pattern))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Số điện thoại không hợp lệ.");
            }
        }
    }
}

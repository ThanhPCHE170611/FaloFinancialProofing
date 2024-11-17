using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Attributes
{
    public class ZipFileValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Không bắt buộc file, chỉ kiểm tra khi có dữ liệu.
            }

            if (value is IFormFile file)
            {
                string fileExtension = Path.GetExtension(file.FileName);

                if (string.Equals(fileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? "File extension must be .zip");
            }

            return new ValidationResult("Invalidate operation.");
        }
    }
}

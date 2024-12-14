using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Attributes
{
    public class ZipFileAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var allowedExtensions = new[] { ".zip", ".pdf" };
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return new ValidationResult("Only zip and pdf files (.zip, .pdf) are allowed.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}

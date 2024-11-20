using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.Attributes
{
    public class ImageFileAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult("Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }

}

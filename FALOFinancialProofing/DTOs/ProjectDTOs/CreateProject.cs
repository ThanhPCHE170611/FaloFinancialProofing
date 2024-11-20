using FALOFinancialProofing.Attributes;
using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.ProjectDTOs
{
    public class CreateProject
    {
        // check if the Id is nullable (v), check exist(..) check if the CreatedBy has role is Project Manager(..)
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Title must be at least 8 and max 100 characters")]
        public string ProjectName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Description must be at least 8 characters")]
        public string Description { get; set; }
        // check if the OrganizationId is valid (exist),
        public int OrganizationId { get; set; }
        // tệp đính kèm
        //[Required(ErrorMessage = "Please select a file.")]
        public List<IFormFile>? FormFiles { get; set; }
        [ImageFile]
        public IFormFile? LogoFile { get; set; }
    }
}

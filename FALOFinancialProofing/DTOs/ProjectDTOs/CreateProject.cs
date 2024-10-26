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
        [StringLength(200)]
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime DateOfCreation { get; set; }
        public bool Status { get; set; }
        // check if the OrganizationId is valid (exist),
        public int OrganizationId { get; set; }
        // tệp đính kèm
        //[Required(ErrorMessage = "Please select a file.")]
        public List<IFormFile>? FormFiles { get; set; }
    }
}

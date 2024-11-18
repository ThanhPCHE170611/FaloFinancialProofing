using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.ProjectDTOs
{
    public class UpdateProjectRequest
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Title must be at least 8 and max 100 characters")]
        public string ProjectName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Description must be at least 8 characters")]
        public string Description { get; set; }
        public IFormFile? LogoFile { get; set; }
    }
}

using FALOFinancialProofing.DTOs.CreateProjectFileDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.DTOs.ProjectDTOs
{
    public class ProjectInformation
    {
        public int id { get; set; }
        public string UserImage { get; set; }
        public string CreatedBy { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string? Logo { get; set; }
        public string? Image { get; set; }
        public ICollection<CreateProjectFileInformation> CreateProjectFiles { get; set; }

    }
}

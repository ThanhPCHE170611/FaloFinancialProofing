using FALOFinancialProofing.Models;
using System.ComponentModel.DataAnnotations;

namespace FALOFinancialProofing.DTOs.ProjectDTOs
{
    public class ProjectInformation
    {
        public string CreatedBy { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}

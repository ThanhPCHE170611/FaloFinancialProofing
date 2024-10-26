using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.ProjectServices
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(Project createProject);
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<bool> UpdateProjectAsync(Project updateProject);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> ValidateProjectCreateAsync(CreateProject createProject, StringBuilder message);
        Task<Project> ConvertDtoToBaseClass(CreateProject createProject);
        Task<Project> CreateProjectReturnEntityAsync(Project createProject);
    }
}

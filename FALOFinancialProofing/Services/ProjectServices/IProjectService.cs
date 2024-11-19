using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Models;
using System.Text;

namespace FALOFinancialProofing.Services.ProjectServices
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(Project createProject);
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectInformation>> GetAllProjectsAsync();
        Task<bool> UpdateProjectAsync(Project updateProject);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> ValidateProjectCreateAsync(CreateProject createProject, StringBuilder message);
        Task<Project> ConvertDtoToBaseClass(CreateProject createProject);
        Task<Project> CreateProjectReturnEntityAsync(Project createProject);
        Task<bool> CheckProjectByUserIdAndProjectIdAsync(string pmUserId, int projectId);
        Task<bool> CheckProjectIsActiveAsync(int projectId);
        Task<List<ProjectInformation>> GetAllProjectsByUserIdAsync(string UserId, HttpRequest request);
        Task<List<ProjectInformation>> GetAllProjectInSystemAsync(HttpRequest request);
        Task<ProjectInformation> GetProjectDetailsByProjectId(int ProjectId, HttpRequest request);
        Task<bool> ValidateProjectUpdateAsync(UpdateProjectRequest updateProjectRequest, StringBuilder message);
        Task<bool> UpdateProjectAsync(UpdateProjectRequest updateProjectRequest, StringBuilder message);
    }
}

using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.ProjectServices
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(Project createProject);
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<bool> UpdateProjectAsync(Project updateProject);
        Task<bool> DeleteProjectAsync(int id);
    }
}

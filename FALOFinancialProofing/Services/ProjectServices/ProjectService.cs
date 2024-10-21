using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.ProjectServices
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project, int> _projectRepository;

        public ProjectService(IRepository<Project, int> projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<bool> CreateProjectAsync(Project createProject)
        {
            try
            {
                if (createProject == null)
                {
                    throw new Exception("Project is null");
                }
                //var organization = ConvertToBaseEntity(createProject);
                await _projectRepository.InsertAsync(createProject);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Project: {ex.Message}!");
            }

            return false;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            Project organization = null!;
            try
            {
                organization = await _projectRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("Project not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetProjectById: {ex.Message}");
            }

            return organization;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            List<Project> data = null!;
            try
            {
                data = await _projectRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllProjects: {ex.Message}");
            }

            return data;
        }


        // admin can update transaction logs
        public async Task<bool> UpdateProjectAsync(Project updateProject)
        {
            Project organization = null!;
            bool result = false;
            try
            {
                organization = await _projectRepository.Get(updateProject.Id);
                if (organization == null)
                {
                    throw new Exception("Project not found!");
                }
                //ConvertToBaseEntity(organization, updateProject);
                result = await _projectRepository.UpdateAsync(updateProject);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateProject: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteProjectAsync(int id)
        {
            Project organization = null!;
            bool result = false;
            try
            {
                organization = await _projectRepository.Get(id);
                if (organization == null)
                {
                    throw new Exception("Project not found!");
                }
                result = await _projectRepository.DeleteAsync(organization);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteProject: {ex.Message}");
            }

            return result;
        }
    }
}

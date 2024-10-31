using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FALOFinancialProofing.Services.ProjectServices
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project, int> _projectRepository;
        private readonly IRepository<Organization, int> _organizationRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AuthServices _authServices;


        public ProjectService(IRepository<Project, int> projectRepository, IRepository<Organization, int> organizationRepository, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AuthServices authServices)
        {
            _projectRepository = projectRepository;
            _organizationRepository = organizationRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _authServices = authServices;
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

        public async Task<Project> CreateProjectReturnEntityAsync(Project createProject)
        {
            try
            {
                if (createProject == null)
                {
                    throw new Exception("Project is null");
                }
                await _projectRepository.InsertAsync(createProject);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Project: {ex.Message}!");
            }

            return createProject;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            Project project = null!;
            try
            {
                project = await _projectRepository.Get(id);
                if (project == null)
                {
                    throw new Exception("Project not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetProjectById: {ex.Message}");
            }

            return project;
        }

        public async Task<List<Project>> GetProjectsByUserIdAsync(string pmUserId)
        {
            List<Project> projects = null!;
            try
            {
                projects = await _projectRepository.GetAll().Where(p => p.CreatedBy == pmUserId).ToListAsync();
                if (projects == null)
                {
                    throw new Exception("projects not found");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetProjectsByUserIdAsync: {ex.Message}");
            }

            return projects;
        }

        public async Task<bool> CheckProjectByUserIdAndProjectIdAsync(string pmUserId, int projectId)
        {
            bool checkValid = false;
            try
            {
                checkValid = (await _projectRepository.GetAll().ToListAsync()).Any(p => p.CreatedBy == pmUserId && p.Id == projectId);
                if (!checkValid)
                {
                    throw new Exception("This user Has no such Project");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CheckProjectByUserIdAndProjectIdAsync: {ex.Message}");
            }

            return checkValid;
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
        public async Task<List<ProjectInformation>> GetAllProjectsByUserIdAsync(string UserId)
        {
            List<ProjectInformation> data = null!;
            try
            {
                data = await _projectRepository.GetAll().Where(p => p.CreatedBy.Equals(UserId))
                    .Select(p => new ProjectInformation()
                    {
                        CreatedBy = p.CreatedBy,
                        ProjectName = p.ProjectName,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        Status = p.Status,
                        IsActive = p.IsActive,
                        OrganizationId = p.OrganizationId,
                        OrganizationName = p.Organization != null ? p.Organization.Name : "No Organization"
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllProjectsByUserIdAsync: {ex.Message}");
            }

            return data;
        }

        public async Task<List<ProjectInformation>> GetAllProjectInSystemAsync()
        {
            List<ProjectInformation> data = null!;
            try
            {
                data = await _projectRepository.GetAll()
                    .Select(p => new ProjectInformation()
                    {
                        CreatedBy = p.CreatedBy,
                        ProjectName = p.ProjectName,
                        Description = p.Description,
                        DateOfCreation = p.DateOfCreation,
                        Status = p.Status,
                        IsActive = p.IsActive,
                        OrganizationId = p.OrganizationId,
                        OrganizationName = p.Organization != null ? p.Organization.Name : "No Organization"
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllProjectInSystemAsync: {ex.Message}");
            }

            return data;
        }
        public async Task<bool> ValidateProjectCreateAsync(CreateProject createProject, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                bool checkValidUser = await _authServices.CheckUserInRole(createProject.CreatedBy, AppRole.ProjectManager, message);
                if (!checkValidUser)
                {
                    return IsValid;
                }
                // !=0 đăng kí với vai trò tổ chức
                if (createProject.OrganizationId != 0)
                {
                    var organization = await _organizationRepository.Get(createProject.OrganizationId);
                    if (organization == null)
                    {
                        throw new Exception("Organization not exist");
                    }
                }
                //trạng thái dự án chưa được phép true
                if (createProject.IsActive)
                {
                    throw new Exception("IsActive must be false");
                }
                // ngày tạo không được lớn hơn ngày hiện tại
                if (createProject.DateOfCreation > DateTime.Now)
                {
                    throw new Exception("Date of creation cannot be in the future");
                }
                IsValid = true;
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"ValidateProjectCreate: {ex.Message}");
            }

            return IsValid;
        }
        public async Task<Project> ConvertDtoToBaseClass(CreateProject createProject)
        {
            Project project = null!;
            try
            {
                project = new Project
                {
                    CreatedBy = createProject.CreatedBy,
                    ProjectName = createProject.ProjectName,
                    Description = createProject.Description,
                    DateOfCreation = createProject.DateOfCreation,
                    Status = createProject.Status,
                    OrganizationId = createProject.OrganizationId != 0 ? createProject.OrganizationId : null
                };
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"ConvertDtoToBaseClass: {ex.Message}");
            }

            return project;
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

        public async Task<bool> CheckProjectIsActiveAsync(int projectId)
        {
            bool checkValid = false;
            try
            {
                checkValid = (await _projectRepository.Get(projectId)).IsActive;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CheckProjectIsActiveAsync: {ex.Message}");
            }
            return checkValid;
        }
    }
}

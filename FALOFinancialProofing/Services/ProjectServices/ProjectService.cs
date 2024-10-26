using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
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
        public readonly UserManager<User> _userManager;

        public ProjectService(IRepository<Project, int> projectRepository, IRepository<Organization, int> organizationRepository, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _projectRepository = projectRepository;
            _organizationRepository = organizationRepository;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<bool> ValidateProjectCreateAsync(CreateProject createProject, StringBuilder message)
        {
            bool IsValid = false;
            try
            {
                // check User is valid (role is PM)
                var user = await _userManager.FindByIdAsync(createProject.CreatedBy);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var isInRole = await _userManager.IsInRoleAsync(user, AppRole.ProjectManager);
                if (!isInRole)
                {
                    throw new Exception("User Role is not permitted");
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
                if (createProject.Status)
                {
                    throw new Exception("Status must be false");
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


    }
}

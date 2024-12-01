using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectFileServices;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.ProjectServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICreateProjectRequestService _createProjectRequestService;
        private readonly ICreateProjectFileService _createProjectFileService;

        public ProjectsController(IProjectService projectService, ICreateProjectRequestService createProjectRequestService, ICreateProjectFileService createProjectFileService)
        {
            _projectService = projectService;
            _createProjectRequestService = createProjectRequestService;
            _createProjectFileService = createProjectFileService;
        }
        //[RoleAttribute(AppRole.ProjectManagementBoard, AppRole.Admin)]
        [HttpGet("GetAllProjectInSystem")]
        public async Task<IActionResult> GetAllProjectInSystem(string? searchInput, string? status, bool? IsActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<ProjectInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await _projectService.GetAllProjectInSystemAsync(Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Project By In System Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.ProjectName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status == status);
                }
                if (IsActive != null)
                {
                    data = data.FindAll(x => x.IsActive == IsActive);
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<ProjectInformation>(data.AsQueryable(), currentPage, IntConstant.PageSizeCustom).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllProjectInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Project In System Successfully!",
                Data = filterPagingData
            });
        }
        //[RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        //PM xem tất cả project của mình
        [HttpGet("GetAllProjectByUserId/{UserId}")]
        public async Task<IActionResult> GetProjectsByUserIdAsync(string? searchInput, string UserId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<ProjectInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await _projectService.GetAllProjectsByUserIdAsync(UserId, Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Project By UserId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.ProjectName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status == status);
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<ProjectInformation>(data.AsQueryable(), currentPage, IntConstant.PageSizeCustom).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetProjectsByUserId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Project By UserId Successfully!",
                Data = filterPagingData
            });
        }
        //[RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpGet("GetProjectDetailsById/{ProjectId}")]
        public async Task<IActionResult> GetProjectDetailsById(int ProjectId)
        {
            var project = await _projectService.GetProjectDetailsByProjectId(ProjectId, Request);
            if (project == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Project with Id = {ProjectId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Project retrieved successfully.",
                Data = project
            });
        }
        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpPut("UpdateProject")]
        public async Task<IActionResult> PutProject([FromForm] UpdateProjectRequest updateProjectRequest)
        {
            StringBuilder message = new StringBuilder();
            bool checkValid = false;
            try
            {
                checkValid = await _projectService.ValidateProjectUpdateAsync(updateProjectRequest, message);
                if (!checkValid)
                {
                    return Ok(new ApiResponse()
                    {
                        Message = message.ToString(),
                        Success = checkValid
                    });
                }
                updateProjectRequest.ProjectName = updateProjectRequest.ProjectName.Trim();
                updateProjectRequest.Description = updateProjectRequest.Description.Trim();
                checkValid = await _projectService.UpdateProjectAsync(updateProjectRequest, message);
                if (checkValid)
                    message.Append("Update Project Successfully!");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"PutProject: Error {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Message = message.ToString(),
                Success = checkValid
            });
        }
        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManager)] // mở nếu làm thật
        [HttpPost("CreateProject", Name = "CreateProject")]
        public async Task<ActionResult<Project>> PostProject([FromForm] CreateProject createProject)
        {
            // không cần thiết check modelState vì hệ thống tự động kiểm tra r
            StringBuilder stringBuilderMessage = new StringBuilder();
            try
            {
                bool checkValidateProject = await _projectService.ValidateProjectCreateAsync(createProject, stringBuilderMessage);
                if (!checkValidateProject)
                {
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                createProject.ProjectName = createProject.ProjectName.Trim();
                createProject.Description = createProject.Description.Trim();
                var project = await _projectService.ConvertDtoToBaseClass(createProject);

                var checkProjectCreated = await _projectService.CreateProjectReturnEntityAsync(project);

                if (checkProjectCreated == null)
                {
                    stringBuilderMessage.Append("Create Project Failed!");
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                // tạo request trước mới tạo fileAttach
                CreateProjectRequest createProjectRequest = new CreateProjectRequest()
                {
                    ProjectId = project.Id,
                    SenderId = createProject.CreatedBy,
                    Title = $"{checkProjectCreated.ProjectName}",
                    CreatedAt = DateTime.Now,
                    Status = RequestStatus.Pending,
                    Description = $"{createProject.Description}",
                };
                var CreateProjectRequestCreated = await _createProjectRequestService.CreateCreateProjectRequestReturnEntityAsync(createProjectRequest);
                if (CreateProjectRequestCreated == null)
                {
                    stringBuilderMessage.Append("Create Project Request Failed!");
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                // Tạo fileYêu cầu có thể có file hoặc không
                if (createProject.FormFiles != null)
                {
                    var CreateProjectFiles = await _createProjectFileService.SaveUploadedFilesAsync(createProject.FormFiles, CreateProjectRequestCreated.Id);
                    //if (CreateProjectFiles.Count == 0)
                    //{
                    //    stringBuilderMessage.Append("Create Project Request files Failed!");
                    //    return Ok(new
                    //    {
                    //        Message = stringBuilderMessage.ToString()
                    //    });
                    //}
                    //else
                    //{
                    bool checkCreate = await _createProjectFileService.CreateCreateProjectFilesAsync(CreateProjectFiles);
                    if (checkCreate == false)
                    {
                        stringBuilderMessage.Append("Create Project Request files Failed!");
                        return Ok(new
                        {
                            Message = stringBuilderMessage.ToString()
                        });
                    }
                    //}
                }
                stringBuilderMessage.Append("Create Project Successfully!");
                //statusMessage = await _projectService.CreateProjectAsync(createProject)
                //    != false ? "Create Project Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                stringBuilderMessage.Append("Create Project Failed!");
                await Console.Out.WriteLineAsync($"PostProject: {ex.Message}");
            }

            return Content(stringBuilderMessage.ToString());
        }

        // DELETE: api/Projects/5
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.Admin)]
        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _projectService.DeleteProjectAsync(id)
                    != false ? "DELETE Project Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE Project Failed!";
                await Console.Out.WriteLineAsync("PostProject: Error");
            }

            return Content(statusMessage);
        }
        [HttpGet("GetFourProjectByFilter")]
        public async Task<IActionResult> GetFourProjectByFilter(bool IsActive, bool OrderByAscending, int numOfElements)
        {
            List<ProjectInformation> data = null;
            try
            {
                //data = (await _projectService.GetAllProjectsAsync()).ToList();
                data = await _projectService.GetAllProjectInSystemAsync(Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetFourProjectByFilter Failed!",
                        Data = data
                    });
                }
                if (IsActive)
                {
                    data = data.FindAll(x => x.IsActive);
                }
                if (OrderByAscending)
                {
                    data = data.OrderBy(o => o.DateOfCreation).ToList();
                }
                else
                {
                    data = data.OrderByDescending(o => o.DateOfCreation).ToList();
                }
                data = data.Take(numOfElements).ToList();

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetFourProjectByFilter: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetFourProjectByFilter Successfully!",
                Data = data
            });
        }

    }
}

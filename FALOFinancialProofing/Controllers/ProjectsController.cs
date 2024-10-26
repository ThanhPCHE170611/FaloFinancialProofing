using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectFileServices;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.AspNetCore.Http;
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

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {

            return Ok(await _projectService.GetAllProjectsAsync());
        }

        // GET: api/Projects/5
        [HttpGet("GetProject/{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateProject")]
        public async Task<IActionResult> PutProject([FromBody] Project UpdateProject)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _projectService.UpdateProjectAsync(UpdateProject) != false ? "Update Project Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update Project Failed!";
                await Console.Out.WriteLineAsync("PutProject: Error");
            }

            return Content(statusMessage);
        }
        [HttpPost("CreateProject1", Name = "CreateProject1")]
        public async Task CreateProject()
        {
            Project project = new Project()
            {
                CreatedBy = "09360c31-c34d-430c-a354-6bc3925e206d",
                ProjectName = "Nguyen Duc Project",
                DateOfCreation = DateTime.Now,
                Description = "This is a project",
                Status = false,
                OrganizationId = 1
            };
            var checkCreate = await _projectService.CreateProjectReturnEntityAsync(project);
        }
        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    Title = "Create Project",
                    CreatedAt = DateTime.Now,
                    Status = "Pending"
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
                    if (CreateProjectFiles.Count == 0)
                    {
                        stringBuilderMessage.Append("Create Project Request files Failed!");
                        return Ok(new
                        {
                            Message = stringBuilderMessage.ToString()
                        });
                    }
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
    }
}

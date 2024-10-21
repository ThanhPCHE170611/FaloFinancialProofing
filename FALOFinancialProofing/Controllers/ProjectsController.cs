using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.ProjectServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateProject", Name = "CreateProject")]
        public async Task<ActionResult<Project>> PostProject([FromBody] Project createProject)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateProject");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _projectService.CreateProjectAsync(createProject)
                    != false ? "Create Project Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create Project Failed!";
                await Console.Out.WriteLineAsync($"PostProject: {ex.Message}");
            }

            return Content(statusMessage);
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

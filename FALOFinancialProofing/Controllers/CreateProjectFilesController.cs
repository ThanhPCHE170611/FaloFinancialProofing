using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectFileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectFilesController : ControllerBase
    {
        private readonly ICreateProjectFileService _createProjectFileService;

        public CreateProjectFilesController(ICreateProjectFileService createProjectFileService)
        {
            _createProjectFileService = createProjectFileService;
        }

        // GET: api/CreateProjectFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateProjectFile>>> GetCreateProjectFiles()
        {
            return Ok(await _createProjectFileService.GetAllCreateProjectFilesAsync());
        }

        // GET: api/CreateProjectFiles/5
        [HttpGet("GetCreateProjectFile/{id}")]
        public async Task<ActionResult<CreateProjectFile>> GetCreateProjectFile(int id)
        {

            var transactionLog = await _createProjectFileService.GetCreateProjectFileByIdAsync(id);

            if (transactionLog == null)
            {
                return NotFound();
            }

            return transactionLog;
        }

        // PUT: api/CreateProjectFiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateProjectFile")]
        public async Task<IActionResult> PutCreateProjectFile([FromBody] CreateProjectFile UpdateCreateProjectFile)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectFileService.UpdateCreateProjectFileAsync(UpdateCreateProjectFile) != false ? "Update CreateProjectFile Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateProjectFile Failed!";
                await Console.Out.WriteLineAsync("PutCreateProjectFile: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateProjectFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateProjectFile", Name = "CreateCreateProjectFile")]
        public async Task<ActionResult<CreateProjectFile>> PostCreateProjectFile([FromBody] CreateProjectFile createCreateProjectFile)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateProjectFile");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectFileService.CreateCreateProjectFileAsync(createCreateProjectFile)
                    != false ? "Create CreateProjectFile Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateProjectFile Failed!";
                await Console.Out.WriteLineAsync($"PostCreateProjectFile: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateProjectFiles/5
        [HttpDelete("DeleteCreateProjectFile/{id}")]
        public async Task<IActionResult> DeleteCreateProjectFile(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createProjectFileService.DeleteCreateProjectFileAsync(id)
                    != false ? "DELETE CreateProjectFile Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateProjectFile Failed!";
                await Console.Out.WriteLineAsync("PostCreateProjectFile: Error");
            }

            return Content(statusMessage);
        }
    }
}

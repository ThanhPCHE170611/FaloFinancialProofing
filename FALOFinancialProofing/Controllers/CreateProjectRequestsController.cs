using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectRequestsController : ControllerBase
    {
        private readonly ICreateProjectRequestService _createProjectRequestService;

        public CreateProjectRequestsController(ICreateProjectRequestService createProjectFileService)
        {
            _createProjectRequestService = createProjectFileService;
        }

        // GET: api/CreateProjectRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateProjectRequest>>> GetCreateProjectRequests()
        {
            return Ok(await _createProjectRequestService.GetAllCreateProjectRequestsAsync());
        }

        // GET: api/CreateProjectRequests/5
        [HttpGet("GetCreateProjectRequest/{id}")]
        public async Task<ActionResult<CreateProjectRequest>> GetCreateProjectRequest(int id)
        {

            var createProjectRequest = await _createProjectRequestService.GetCreateProjectRequestByIdAsync(id);

            if (createProjectRequest == null)
            {
                return NotFound();
            }

            return createProjectRequest;
        }

        // PUT: api/CreateProjectRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateProjectRequest")]
        public async Task<IActionResult> PutCreateProjectRequest([FromBody] CreateProjectRequest UpdateCreateProjectRequest)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectRequestService.UpdateCreateProjectRequestAsync(UpdateCreateProjectRequest) != false ? "Update CreateProjectRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync("PutCreateProjectRequest: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateProjectRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateProjectRequest", Name = "CreateCreateProjectRequest")]
        public async Task<ActionResult<CreateProjectRequest>> PostCreateProjectRequest([FromBody] CreateProjectRequest createCreateProjectRequest)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateProjectRequest");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectRequestService.CreateCreateProjectRequestAsync(createCreateProjectRequest)
                    != false ? "Create CreateProjectRequest Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync($"PostCreateProjectRequest: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateProjectRequests/5
        [HttpDelete("DeleteCreateProjectRequest/{id}")]
        public async Task<IActionResult> DeleteCreateProjectRequest(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createProjectRequestService.DeleteCreateProjectRequestAsync(id)
                    != false ? "DELETE CreateProjectRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync("PostCreateProjectRequest: Error");
            }

            return Content(statusMessage);
        }
    }
}

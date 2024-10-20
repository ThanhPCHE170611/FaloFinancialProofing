using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCampaignRequestsController : ControllerBase
    {
        private readonly ICreateCampaignRequestService _createCampaignRequestService;

        public CreateCampaignRequestsController(ICreateCampaignRequestService createCampaignFileService)
        {
            _createCampaignRequestService = createCampaignFileService;
        }

        // GET: api/CreateCampaignRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateCampaignRequest>>> GetCreateCampaignRequests()
        {
            return Ok(await _createCampaignRequestService.GetAllCreateCampaignRequestsAsync());
        }

        // GET: api/CreateCampaignRequests/5
        [HttpGet("GetCreateCampaignRequest/{id}")]
        public async Task<ActionResult<CreateCampaignRequest>> GetCreateCampaignRequest(int id)
        {

            var createCampaignRequest = await _createCampaignRequestService.GetCreateCampaignRequestByIdAsync(id);

            if (createCampaignRequest == null)
            {
                return NotFound();
            }

            return createCampaignRequest;
        }

        // PUT: api/CreateCampaignRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateCampaignRequest")]
        public async Task<IActionResult> PutCreateCampaignRequest([FromBody] CreateCampaignRequest UpdateCreateCampaignRequest)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignRequestService.UpdateCreateCampaignRequestAsync(UpdateCreateCampaignRequest) != false ? "Update CreateCampaignRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync("PutCreateCampaignRequest: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateCampaignRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateCampaignRequest", Name = "CreateCreateCampaignRequest")]
        public async Task<ActionResult<CreateCampaignRequest>> PostCreateCampaignRequest([FromBody] CreateCampaignRequest createCreateCampaignRequest)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateCampaignRequest");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignRequestService.CreateCreateCampaignRequestAsync(createCreateCampaignRequest)
                    != false ? "Create CreateCampaignRequest Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync($"PostCreateCampaignRequest: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateCampaignRequests/5
        [HttpDelete("DeleteCreateCampaignRequest/{id}")]
        public async Task<IActionResult> DeleteCreateCampaignRequest(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createCampaignRequestService.DeleteCreateCampaignRequestAsync(id)
                    != false ? "DELETE CreateCampaignRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync("PostCreateCampaignRequest: Error");
            }

            return Content(statusMessage);
        }
    }
}

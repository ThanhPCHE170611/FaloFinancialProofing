using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveNextCampaignStatusRequestsController : ControllerBase
    {
        private readonly IMoveNextCampaignStatusRequestService _moveNextCampaignStatusRequestService;

        public MoveNextCampaignStatusRequestsController(IMoveNextCampaignStatusRequestService moveNextCampaignStatusRequestService)
        {
            _moveNextCampaignStatusRequestService = moveNextCampaignStatusRequestService;
        }

        // GET: api/MoveNextCampaignStatusRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoveNextCampaignStatusRequest>>> GetMoveNextCampaignStatusRequests()
        {
            return Ok(await _moveNextCampaignStatusRequestService.GetAllMoveNextCampaignStatusRequestsAsync());
        }

        // GET: api/MoveNextCampaignStatusRequests/5
        [HttpGet("GetMoveNextCampaignStatusRequest/{id}")]
        public async Task<ActionResult<MoveNextCampaignStatusRequest>> GetMoveNextCampaignStatusRequest(int id)
        {

            var createCampaignRequest = await _moveNextCampaignStatusRequestService.GetMoveNextCampaignStatusRequestByIdAsync(id);

            if (createCampaignRequest == null)
            {
                return NotFound();
            }

            return createCampaignRequest;
        }

        // PUT: api/MoveNextCampaignStatusRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateMoveNextCampaignStatusRequest")]
        public async Task<IActionResult> PutMoveNextCampaignStatusRequest([FromBody] MoveNextCampaignStatusRequest UpdateMoveNextCampaignStatusRequest)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _moveNextCampaignStatusRequestService.UpdateMoveNextCampaignStatusRequestAsync(UpdateMoveNextCampaignStatusRequest) != false ? "Update MoveNextCampaignStatusRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update MoveNextCampaignStatusRequest Failed!";
                await Console.Out.WriteLineAsync("PutMoveNextCampaignStatusRequest: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/MoveNextCampaignStatusRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateMoveNextCampaignStatusRequest", Name = "CreateMoveNextCampaignStatusRequest")]
        public async Task<ActionResult<MoveNextCampaignStatusRequest>> PostMoveNextCampaignStatusRequest([FromBody] MoveNextCampaignStatusRequest createMoveNextCampaignStatusRequest)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateMoveNextCampaignStatusRequest");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _moveNextCampaignStatusRequestService.CreateMoveNextCampaignStatusRequestAsync(createMoveNextCampaignStatusRequest)
                    != false ? "Create MoveNextCampaignStatusRequest Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create MoveNextCampaignStatusRequest Failed!";
                await Console.Out.WriteLineAsync($"PostMoveNextCampaignStatusRequest: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/MoveNextCampaignStatusRequests/5
        [HttpDelete("DeleteMoveNextCampaignStatusRequest/{id}")]
        public async Task<IActionResult> DeleteMoveNextCampaignStatusRequest(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _moveNextCampaignStatusRequestService.DeleteMoveNextCampaignStatusRequestAsync(id)
                    != false ? "DELETE MoveNextCampaignStatusRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE MoveNextCampaignStatusRequest Failed!";
                await Console.Out.WriteLineAsync("PostMoveNextCampaignStatusRequest: Error");
            }

            return Content(statusMessage);
        }
    }
}

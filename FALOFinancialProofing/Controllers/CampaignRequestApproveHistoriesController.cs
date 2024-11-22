using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.DTOs.CampaignRequestApproveHistoryDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CampaignRequestApproveHistoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignRequestApproveHistoriesController : ControllerBase
    {
        private readonly ICampaignRequestApproveHistoryService _campaignRequestApproveHistoryService;

        public CampaignRequestApproveHistoriesController(ICampaignRequestApproveHistoryService campaignRequestApproveHistoryService)
        {
            _campaignRequestApproveHistoryService = campaignRequestApproveHistoryService;
        }

        // GET: api/CampaignRequestApproveHistorys
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CampaignRequestApproveHistory>>> GetCampaignRequestApproveHistorys()
        //{
        //    return Ok(await _campaignRequestApproveHistoryService.GetAllCampaignRequestApproveHistorysAsync());
        //}

        // GET: api/CampaignRequestApproveHistorys/5
        [HttpGet("GetCampaignRequestApproveHistory/{id}")]
        public async Task<ActionResult<CampaignRequestApproveHistory>> GetCampaignRequestApproveHistory(int id)
        {

            var organization = await _campaignRequestApproveHistoryService.GetCampaignRequestApproveHistoryByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }
        // PUT: api/CampaignRequestApproveHistorys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCampaignRequestApproveHistory")]
        public async Task<IActionResult> PutCampaignRequestApproveHistory([FromBody] CampaignRequestApproveHistory UpdateCampaignRequestApproveHistory)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _campaignRequestApproveHistoryService.UpdateCampaignRequestApproveHistoryAsync(UpdateCampaignRequestApproveHistory) != false ? "Update CampaignRequestApproveHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CampaignRequestApproveHistory Failed!";
                await Console.Out.WriteLineAsync("PutCampaignRequestApproveHistory: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CampaignRequestApproveHistorys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpPost("CreateCampaignRequestApproveHistory", Name = "CreateCampaignRequestApproveHistory")]
        public async Task<ActionResult<CampaignRequestApproveHistory>> PostCampaignRequestApproveHistory([FromBody] CampaignRequestApproveHistoryClientRequest campaignRequestApproveHistoryClientRequest)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                bool checkValid = await _campaignRequestApproveHistoryService.CheckValidCampaignRequestApproveHistory(campaignRequestApproveHistoryClientRequest, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                campaignRequestApproveHistoryClientRequest.FeedBack = campaignRequestApproveHistoryClientRequest.FeedBack?.Trim();
                checkValid = await _campaignRequestApproveHistoryService.CreateCampaignRequestApproveHistoryAsync(campaignRequestApproveHistoryClientRequest, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                message.Append("Create CampaignRequestApproveHistory Successfully!");
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"PostCampaignRequestApproveHistory: {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString()
            });
        }

        // DELETE: api/CampaignRequestApproveHistorys/5
        [HttpDelete("DeleteCampaignRequestApproveHistory/{id}")]
        public async Task<IActionResult> DeleteCampaignRequestApproveHistory(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _campaignRequestApproveHistoryService.DeleteCampaignRequestApproveHistoryAsync(id)
                    != false ? "DELETE CampaignRequestApproveHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CampaignRequestApproveHistory Failed!";
                await Console.Out.WriteLineAsync("PostCampaignRequestApproveHistory: Error");
            }

            return Content(statusMessage);
        }
    }
}

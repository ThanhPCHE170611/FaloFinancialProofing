using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestHistoryDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestHistoryService;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class MoveNextCampaignStatusRequestHistoryController : ControllerBase
    {
        private readonly IMoveNextCampaignStatusRequestHistoryService _moveNextCampaignStatusRequestHistoryService;

        public MoveNextCampaignStatusRequestHistoryController(IMoveNextCampaignStatusRequestHistoryService moveNextCampaignStatusRequestHistoryService)
        {
            _moveNextCampaignStatusRequestHistoryService = moveNextCampaignStatusRequestHistoryService;
        }


        [HttpGet("GetMoveNextCampaignStatusRequestHistory/{id}")]
        public async Task<ActionResult<MoveNextCampaignStatusRequestHistory>> GetMoveNextCampaignStatusRequestHistory(int id)
        {

            var moveNextCampaignStatusRequestHistory = await _moveNextCampaignStatusRequestHistoryService.GetMoveNextCampaignStatusRequestHistoryByIdAsync(id);

            if (moveNextCampaignStatusRequestHistory == null)
            {
                return NotFound();
            }

            return moveNextCampaignStatusRequestHistory;
        }

        [HttpPut("UpdateMoveNextCampaignStatusRequestHistory")]
        public async Task<IActionResult> PutMoveNextCampaignStatusRequestHistory([FromBody] MoveNextCampaignStatusRequestHistory UpdateMoveNextCampaignStatusRequestHistory)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _moveNextCampaignStatusRequestHistoryService.UpdateMoveNextCampaignStatusRequestHistoryAsync(UpdateMoveNextCampaignStatusRequestHistory) != false ? "Update MoveNextCampaignStatusRequestHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update MoveNextCampaignStatusRequestHistory Failed!";
                await Console.Out.WriteLineAsync("PutMoveNextCampaignStatusRequestHistory: Error");
            }

            return Content(statusMessage);
        }



        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpPost("CreateMoveNextCampaignStatusRequestHistory")]
        public async Task<ActionResult<MoveNextCampaignStatusRequestHistory>> MoveNextCampaignStatusRequestHistory([FromBody] MoveNextCampaignStatusRequestHistoryDTO requestHistoryDTO)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                bool checkValid = await _moveNextCampaignStatusRequestHistoryService.CheckValidMoveNextCampaignStatusRequestHistory(requestHistoryDTO, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                checkValid = await _moveNextCampaignStatusRequestHistoryService.CreateMoveNextCampaignStatusRequestHistoryAsync(requestHistoryDTO, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                message.Append("Create MoveNextCampaignStatusRequestHistory Successfully!");
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"MoveNextCampaignStatusRequestHistory: {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString()
            });
        }

        [HttpDelete("DeleteMoveNextCampaignStatusRequestHistory/{id}")]
        public async Task<IActionResult> DeleteMoveNextCampaignStatusRequestHistory(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _moveNextCampaignStatusRequestHistoryService.DeleteMoveNextCampaignStatusRequestHistoryAsync(id)
                    != false ? "DELETE MoveNextCampaignStatusRequestHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE MoveNextCampaignStatusRequestHistory Failed!";
                await Console.Out.WriteLineAsync("MoveNextCampaignStatusRequestHistory: Error");
            }

            return Content(statusMessage);
        }
    }
}

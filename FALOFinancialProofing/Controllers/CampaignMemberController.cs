using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignMemberController : ControllerBase
    {
        private readonly ICampaignMemberService _campaignMemberService;

        public CampaignMemberController(ICampaignMemberService campaignMemberService)
        {
            _campaignMemberService = campaignMemberService;
        }

        [HttpGet("GetAllCampaignMember")]
        public async Task<IActionResult> GetAllCampaignMember()
        {
            var campaignMembers = await _campaignMemberService.GetAllCampaignMembersAsync();
            if (campaignMembers == null || campaignMembers.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No CampaignMembers found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMembers retrieved successfully.",
                Data = campaignMembers
            });
        }

        [HttpGet("GetCampaignMemberByCampaignIdAndUserId/{campaignId},{userId}")]
        public async Task<IActionResult> GetCampaignMemberByCampaignIdAndUserId(int campaignId, string userId)
        {
            var campaignMember = await _campaignMemberService.GetCampaignMemberByCampaignIdAndUserIdAsync(campaignId, userId);
            if (campaignMember == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with CampaignId = {campaignId} and UserId = {userId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember retrieved successfully.",
                Data = campaignMember
            });
        }

        [HttpPost("CreateCampaignMember")]
        public async Task<IActionResult> CreateCampaignMember([FromBody] CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            var createCampaignMember = await _campaignMemberService.CreateCampaignMemberAsync(createCampaignMemberDTO);
            if (createCampaignMember == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create CampaignMember."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember created successfully.",
                Data = createCampaignMember
            });
        }

        [HttpPut("UpdateCampaignMember")]
        public async Task<IActionResult> UpdateCampaignMember([FromBody] UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {

            var canUpdateCampaignMember = await _campaignMemberService.UpdateCampaignMemberAsync(updateCampaignMemberDTO);
            if (!canUpdateCampaignMember)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with CampaignId = {updateCampaignMemberDTO.CampaignId} and UserId = {updateCampaignMemberDTO.UserId} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember updated successfully."
            });
        }
        [HttpDelete("DeleteCampaignMemberByCampaignIdAndUserId/{campaignId},{userId}")]
        public async Task<IActionResult> DeleteCampaignMemberByCampaignIdAndUserId(int campaignId, string userId)
        {
            var deleteResult = await _campaignMemberService.DeleteCampaignMemberByCampaignIdAndUserIdAsync(campaignId, userId);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with CampaignId = {campaignId} and UserId = {userId} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember deleted successfully."
            });
        }
    }
}

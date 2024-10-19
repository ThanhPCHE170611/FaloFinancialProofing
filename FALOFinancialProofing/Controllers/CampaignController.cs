using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.SocialNetworkService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("GetAllCampaign")]
        public async Task<IActionResult> GetAllCampaign()
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync();
            if (campaigns == null || campaigns.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Campaigns found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaigns retrieved successfully.",
                Data = campaigns
            });
        }

        [HttpGet("GetCampaignById/{id}")]
        public async Task<IActionResult> GetCampaignById(int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign retrieved successfully.",
                Data = campaign
            });
        }

        [HttpPost("CreateCampaign")]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDTO createCampaignDTO)
        {
            var createCampaign = await _campaignService.CreateCampaignAsync(createCampaignDTO);
            if (createCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create Campaign."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign created successfully.",
                Data = createCampaign
            });
        }

        [HttpPut("UpdateCampaign/{id}")]
        public async Task<IActionResult> UpdateCampaign([FromBody] UpdateCampaignDTO updateCampaignDTO)
        {

            var canUpdateCampaign = await _campaignService.UpdateCampaignAsync(updateCampaignDTO);
            if (!canUpdateCampaign)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {updateCampaignDTO.Id} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign updated successfully."
            });
        }
        [HttpDelete("DeleteCampaignById/{id}")]
        public async Task<IActionResult> DeleteCampaignById(int id)
        {
            var deleteResult = await _campaignService.DeleteCampaignByIdAsync(id);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {id} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign deleted successfully."
            });
        }

        
    }
}

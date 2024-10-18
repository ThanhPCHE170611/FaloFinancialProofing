using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Services.SocialNetworkService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialNetworkController : ControllerBase
    {
        private readonly ISocialNetworkService _socialNetworkService;

        public SocialNetworkController(ISocialNetworkService socialNetworkService)
        {
            _socialNetworkService = socialNetworkService;
        }

        [HttpGet("GetAllSocialNetworks")]
        public async Task<IActionResult> GetAllSocialNetworks()
        {
            var socialNetworks = await _socialNetworkService.GetAllSocialNetworksAsync();
            if (socialNetworks == null || socialNetworks.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No SocialNetworks found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetworks retrieved successfully.",
                Data = socialNetworks
            });
        }

        [HttpGet("GetSocialNetworkById/{id}")]
        public async Task<IActionResult> GetSocialNetworkById(int id)
        {
            var socialNetwork = await _socialNetworkService.GetSocialNetworkByIdAsync(id);
            if (socialNetwork == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SocialNetwork with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork retrieved successfully.",
                Data = socialNetwork
            });
        }

        [HttpGet("GetSocialNetworkByUserId/{userId}")]
        public async Task<IActionResult> GetSocialNetworkByUserId(string userId)
        {
            var socialNetwork = await _socialNetworkService.GetSocialNetworkByUserIdAsync(userId);
            if (socialNetwork == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SocialNetwork for User Id = {userId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork retrieved successfully.",
                Data = socialNetwork
            });
        }

        // POST: api/SocialNetwork
        [HttpPost("CreateSocialNetwork")]
        public async Task<IActionResult> CreateSocialNetwork([FromBody] SocialNetworkRequest socialNetworkRequest)
        {
            var createdSocialNetwork = await _socialNetworkService.CreateSocialNetworkAsync(socialNetworkRequest);
            if (createdSocialNetwork == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create SocialNetwork."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork created successfully.",
                Data = createdSocialNetwork
            });
        }

        [HttpPut("UpdateSocialNetwork/{id}")]
        public async Task<IActionResult> UpdateSocialNetwork([FromBody] SocialNetworkRequest socialNetworkRequest)
        {

            var canUpdateSocialNetwork = await _socialNetworkService.UpdateSocialNetworkAsync(socialNetworkRequest);
            if (!canUpdateSocialNetwork)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SocialNetwork with Id = {socialNetworkRequest.Id} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork updated successfully."
            });
        }
        [HttpDelete("DeleteSocialNetworkById/{id}")]
        public async Task<IActionResult> DeleteSocialNetworkById(int id)
        {
            var deleteResult = await _socialNetworkService.DeleteSocialNetworkByIdAsync(id);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SocialNetwork with Id = {id} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork deleted successfully."
            });
        }

        [HttpDelete("DeleteSocialNetwork")]
        public async Task<IActionResult> DeleteSocialNetwork([FromBody] SocialNetworkRequest socialNetworkRequest)
        {
            var deleteResult = await _socialNetworkService.DeleteSocialNetworkAsync(socialNetworkRequest);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to delete SocialNetwork."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SocialNetwork deleted successfully."
            });
        }
    }
}

using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.SDGServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FALOFinancialProofing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SDGController : ControllerBase
    {
        private readonly ISDGServices _sdgServices;

        public SDGController(ISDGServices sdgServices)
        {
            _sdgServices = sdgServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSDGs()
        {
            var sdgs = await _sdgServices.GetAllSDGsAsync();
            if (sdgs == null || sdgs.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No SDGs found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDGs retrieved successfully.",
                Data = sdgs
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSDGById(int id)
        {
            var sdg = await _sdgServices.GetSDGByIdAsync(id);
            if (sdg == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SDG with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG retrieved successfully.",
                Data = sdg
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSDGByUserId(string userId)
        {
            var sdg = await _sdgServices.GetSDGByUserIdAsync(userId);
            if (sdg == null )
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SDG for User Id = {userId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG retrieved successfully.",
                Data = sdg
            });
        }

        // POST: api/SDG
        [HttpPost]
        public async Task<IActionResult> CreateSDG([FromBody] SDGRequest sdg)
        {
            var createdSDG = await _sdgServices.CreateSDGAsync(sdg);
            if (createdSDG == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create SDG."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG created successfully.",
                Data = createdSDG
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSDG([FromBody] SDGRequest sdg)
        {

            var canUpdateSDG = await _sdgServices.UpdateSDGAsync(sdg);
            if (!canUpdateSDG)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SDG with Id = {sdg.Id} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG updated successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSDGById(int id)
        {
            var deleteResult = await _sdgServices.DeleteSDGByIdAsync(id);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"SDG with Id = {id} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG deleted successfully."
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSDG([FromBody] SDGRequest sdg)
        {
            var deleteResult = await _sdgServices.DeleteSDGAsync(sdg);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to delete SDG."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "SDG deleted successfully."
            });
        }
    }
}

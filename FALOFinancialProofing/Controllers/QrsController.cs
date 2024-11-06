using FALOFinancialProofing.Constant;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrsController : ControllerBase
    {
        [HttpGet("GetQRCode")]
        public async Task<IActionResult> GetQRAsync()
        {
     
            try
            {
               
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetQRAsync: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                //Success = true,
                //Message = "Get All Project By UserId Successfully!",
                //Data = filterPagingData
            });
        }
    }
}

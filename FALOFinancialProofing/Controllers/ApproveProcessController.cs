using FALOFinancialProofing.Services.ApproveProcessServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveProcessController : ControllerBase
    {
        private readonly IApproveProcessServices approveProcessServices;

        public ApproveProcessController(IApproveProcessServices approveProcessServices)
        {
            this.approveProcessServices = approveProcessServices;
        }

        [HttpGet("getallprepayrequestforvolunteerleader/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForVolunteerLeader(string userid, string currentLoggingRole)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForVolunteerLeader(userid, currentLoggingRole);
            return Ok(result);
        }
    }
}

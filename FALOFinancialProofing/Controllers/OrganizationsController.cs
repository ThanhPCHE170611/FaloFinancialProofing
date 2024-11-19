using FALOFinancialProofing.DTOs.OrganizationDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using FALOFinancialProofing.Services.OrganizationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IOrganizationMemberService _organizationMemberService;

        public OrganizationsController(IOrganizationService organizationService, IOrganizationMemberService organizationMemberService)
        {
            _organizationService = organizationService;
            _organizationMemberService = organizationMemberService;
        }

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizations()
        {
            return Ok(await _organizationService.GetAllOrganizationsAsync());
        }

        // GET: api/Organizations/5
        [HttpGet("GetOrganization/{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(int id)
        {

            var organization = await _organizationService.GetOrganizationByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }


        [HttpGet("GetOrganizationsByUserId/{userId}")]
        public async Task<ActionResult<List<Organization>>> GetOrganizationsByUserId(string userId)
        {
            var organization = await _organizationService.GetOrganizationsByUserIdAsync(userId);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }
        // PUT: api/Organizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateOrganization")]
        public async Task<IActionResult> PutOrganization([FromBody] Organization UpdateOrganization)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _organizationService.UpdateOrganizationAsync(UpdateOrganization) != false ? "Update Organization Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update Organization Failed!";
                await Console.Out.WriteLineAsync("PutOrganization: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/Organizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateOrganization", Name = "CreateOrganization")]
        public async Task<ActionResult> PostOrganization([FromForm] CreateOrganization createOrganization)
        {
            StringBuilder statusMessage = new StringBuilder();
            Organization createOrganizationResult = null!;
            bool checkValid = false;
            try
            {
                checkValid = await _organizationService.ValidateCreateOrganizationAsync(createOrganization, statusMessage);
                if (!checkValid)
                {
                    return Ok(new ApiResponse
                    {
                        Success = checkValid,
                        Message = statusMessage.ToString()
                    });
                }
                createOrganizationResult = await _organizationService.CreateOrganizationAsync(createOrganization, statusMessage);
                await _organizationMemberService.CreateOrganizationMemberAsync(new OrganizationMember
                {
                    UserId = createOrganization.UserId,
                    OrganizationId = createOrganizationResult.Id,
                });
                checkValid = true;
            }
            catch (Exception ex)
            {
                statusMessage.Append(ex.Message);
                await Console.Out.WriteLineAsync($"PostOrganization: {ex.Message}");
            }

            return Ok(new ApiResponse
            {
                Success = checkValid,
                Message = statusMessage.ToString()
            });
        }

        // DELETE: api/Organizations/5
        [HttpDelete("DeleteOrganization/{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _organizationService.DeleteOrganizationAsync(id)
                    != false ? "DELETE Organization Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE Organization Failed!";
                await Console.Out.WriteLineAsync("PostOrganization: Error");
            }

            return Content(statusMessage);
        }
    }
}

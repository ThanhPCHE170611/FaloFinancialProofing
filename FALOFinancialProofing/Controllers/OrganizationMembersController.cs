using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationMembersController : ControllerBase
    {
        private readonly IOrganizationMemberService _organizationMemberService;

        public OrganizationMembersController(IOrganizationMemberService organizationService)
        {
            _organizationMemberService = organizationService;
        }

        // GET: api/OrganizationMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationMember>>> GetOrganizationMembers()
        {
            return Ok(await _organizationMemberService.GetAllOrganizationMembersAsync());
        }

        // GET: api/OrganizationMembers/5
        [HttpGet("GetOrganizationMember/{id}")]
        public async Task<ActionResult<OrganizationMember>> GetOrganizationMember(int id)
        {

            var organization = await _organizationMemberService.GetOrganizationMemberByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }

        // PUT: api/OrganizationMembers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateOrganizationMember")]
        public async Task<IActionResult> PutOrganizationMember([FromBody] OrganizationMember UpdateOrganizationMember)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _organizationMemberService.UpdateOrganizationMemberAsync(UpdateOrganizationMember) != false ? "Update OrganizationMember Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update OrganizationMember Failed!";
                await Console.Out.WriteLineAsync("PutOrganizationMember: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/OrganizationMembers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateOrganizationMember", Name = "CreateOrganizationMember")]
        public async Task<ActionResult<OrganizationMember>> PostOrganizationMember([FromBody] OrganizationMember createOrganizationMember)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateOrganizationMember");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _organizationMemberService.CreateOrganizationMemberAsync(createOrganizationMember)
                    != false ? "Create OrganizationMember Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create OrganizationMember Failed!";
                await Console.Out.WriteLineAsync($"PostOrganizationMember: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/OrganizationMembers/5
        [HttpDelete("DeleteOrganizationMember/{id}")]
        public async Task<IActionResult> DeleteOrganizationMember(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _organizationMemberService.DeleteOrganizationMemberAsync(id)
                    != false ? "DELETE OrganizationMember Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE OrganizationMember Failed!";
                await Console.Out.WriteLineAsync("PostOrganizationMember: Error");
            }

            return Content(statusMessage);
        }
    }
}

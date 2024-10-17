using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.OrganizationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
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
        public async Task<ActionResult<Organization>> PostOrganization([FromBody] Organization createOrganization)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateOrganization");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _organizationService.CreateOrganizationAsync(createOrganization)
                    != false ? "Create Organization Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create Organization Failed!";
                await Console.Out.WriteLineAsync($"PostOrganization: {ex.Message}");
            }

            return Content(statusMessage);
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

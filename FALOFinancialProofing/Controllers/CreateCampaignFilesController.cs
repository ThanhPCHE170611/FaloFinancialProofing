using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateCampaignFileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCampaignFilesController : ControllerBase
    {
        private readonly ICreateCampaignFileService _createCampaignFileService;

        public CreateCampaignFilesController(ICreateCampaignFileService createCampaignFileService)
        {
            _createCampaignFileService = createCampaignFileService;
        }

        // GET: api/CreateCampaignFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateCampaignFile>>> GetCreateCampaignFiles()
        {
            return Ok(await _createCampaignFileService.GetAllCreateCampaignFilesAsync());
        }

        // GET: api/CreateCampaignFiles/5
        [HttpGet("GetCreateCampaignFile/{id}")]
        public async Task<ActionResult<CreateCampaignFile>> GetCreateCampaignFile(int id)
        {

            var transactionLog = await _createCampaignFileService.GetCreateCampaignFileByIdAsync(id);

            if (transactionLog == null)
            {
                return NotFound();
            }

            return transactionLog;
        }

        // PUT: api/CreateCampaignFiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateCampaignFile")]
        public async Task<IActionResult> PutCreateCampaignFile([FromBody] CreateCampaignFile UpdateCreateCampaignFile)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignFileService.UpdateCreateCampaignFileAsync(UpdateCreateCampaignFile) != false ? "Update CreateCampaignFile Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateCampaignFile Failed!";
                await Console.Out.WriteLineAsync("PutCreateCampaignFile: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateCampaignFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateCampaignFile", Name = "CreateCreateCampaignFile")]
        public async Task<ActionResult<CreateCampaignFile>> PostCreateCampaignFile([FromBody] CreateCampaignFile createCreateCampaignFile)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateCampaignFile");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignFileService.CreateCreateCampaignFileAsync(createCreateCampaignFile)
                    != false ? "Create CreateCampaignFile Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateCampaignFile Failed!";
                await Console.Out.WriteLineAsync($"PostCreateCampaignFile: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateCampaignFiles/5
        [HttpDelete("DeleteCreateCampaignFile/{id}")]
        public async Task<IActionResult> DeleteCreateCampaignFile(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createCampaignFileService.DeleteCreateCampaignFileAsync(id)
                    != false ? "DELETE CreateCampaignFile Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateCampaignFile Failed!";
                await Console.Out.WriteLineAsync("PostCreateCampaignFile: Error");
            }

            return Content(statusMessage);
        }
    }
}

using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.DTOs.CreateProjectRequestApproveHistoryDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectRequestApproveHistoriesController : ControllerBase
    {
        private readonly ICreateProjectRequestApproveHistoryService _createProjectRequestApproveHistoryService;

        public CreateProjectRequestApproveHistoriesController(ICreateProjectRequestApproveHistoryService createProjectRequestApproveHistoryService)
        {
            _createProjectRequestApproveHistoryService = createProjectRequestApproveHistoryService;
        }

        // GET: api/CreateProjectRequestApproveHistorys
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CreateProjectRequestApproveHistory>>> GetCreateProjectRequestApproveHistorys()
        //{
        //    return Ok(await _createProjectRequestApproveHistoryService.GetAllCreateProjectRequestApproveHistorysAsync());
        //}

        // GET: api/CreateProjectRequestApproveHistorys/5
        [HttpGet("GetCreateProjectRequestApproveHistory/{id}")]
        public async Task<ActionResult<CreateProjectRequestApproveHistory>> GetCreateProjectRequestApproveHistory(int id)
        {

            var organization = await _createProjectRequestApproveHistoryService.GetCreateProjectRequestApproveHistoryByIdAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }
        // PUT: api/CreateProjectRequestApproveHistorys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateProjectRequestApproveHistory")]
        public async Task<IActionResult> PutCreateProjectRequestApproveHistory([FromBody] CreateProjectRequestApproveHistory UpdateCreateProjectRequestApproveHistory)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectRequestApproveHistoryService.UpdateCreateProjectRequestApproveHistoryAsync(UpdateCreateProjectRequestApproveHistory) != false ? "Update CreateProjectRequestApproveHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateProjectRequestApproveHistory Failed!";
                await Console.Out.WriteLineAsync("PutCreateProjectRequestApproveHistory: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateProjectRequestApproveHistorys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpPost("CreateCreateProjectRequestApproveHistory", Name = "CreateCreateProjectRequestApproveHistory")]
        public async Task<ActionResult<CreateProjectRequestApproveHistory>> PostCreateProjectRequestApproveHistory([FromBody] CreateProjectRequestApproveHistoryClientRequest createProjectRequestApproveHistoryClientRequest)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                bool checkValid = await _createProjectRequestApproveHistoryService.CheckValidCreateProjectRequestApproveHistory(createProjectRequestApproveHistoryClientRequest, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                createProjectRequestApproveHistoryClientRequest.FeedBack = createProjectRequestApproveHistoryClientRequest.FeedBack?.Trim();
                checkValid = await _createProjectRequestApproveHistoryService.CreateCreateProjectRequestApproveHistoryAsync(createProjectRequestApproveHistoryClientRequest, message);
                if (!checkValid)
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                message.Append("Create CreateProjectRequestApproveHistory Successfully!");
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                await Console.Out.WriteLineAsync($"PostCreateProjectRequestApproveHistory: {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString()
            });
        }

        // DELETE: api/CreateProjectRequestApproveHistorys/5
        [HttpDelete("DeleteCreateProjectRequestApproveHistory/{id}")]
        public async Task<IActionResult> DeleteCreateProjectRequestApproveHistory(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createProjectRequestApproveHistoryService.DeleteCreateProjectRequestApproveHistoryAsync(id)
                    != false ? "DELETE CreateProjectRequestApproveHistory Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateProjectRequestApproveHistory Failed!";
                await Console.Out.WriteLineAsync("PostCreateProjectRequestApproveHistory: Error");
            }

            return Content(statusMessage);
        }
    }
}

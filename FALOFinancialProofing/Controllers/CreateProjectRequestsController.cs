using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectRequestsController : ControllerBase
    {
        private readonly ICreateProjectRequestService _createProjectRequestService;

        public CreateProjectRequestsController(ICreateProjectRequestService createProjectFileService)
        {
            _createProjectRequestService = createProjectFileService;
        }

        // GET: api/CreateProjectRequests
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CreateProjectRequest>>> GetCreateProjectRequests()
        //{
        //    return Ok(await _createProjectRequestService.GetAllCreateProjectRequestsAsync());
        //}

        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpGet("GetCreateProjectRequestByPMB")]
        public async Task<ActionResult<IEnumerable<CreateProjectRequestInformation>>> GetCreateProjectRequests()
        {
            StringBuilder message = new StringBuilder();
            IEnumerable<CreateProjectRequestInformation> data = null;
            try
            {
                data = await _createProjectRequestService.GetAllCreateProjectRequestsByPMBAsync(message);
                message.Append("Get CreateProjectRequests Successfully!");
            }
            catch (Exception ex)
            {
                message.Append("Get CreateProjectRequests Failed!");
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString(),
                Data = data
            });
        }

        [HttpGet("GetAllProjectsByUserId/{ProjectId}")]
        public async Task<IActionResult> GetAllProjectsByUserId(int UserId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = await _campaignService.GetAllCampaignsByProjectIdAsync(ProjectId);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Campaign By ProjectId Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status == status);
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<CampaignInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignByProjectId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Campaign By ProjectId Successfully!",
                Data = filterPagingData
            });
        }
        // GET: api/CreateProjectRequests/5
        [HttpGet("GetCreateProjectRequest/{id}")]
        public async Task<ActionResult<CreateProjectRequest>> GetCreateProjectRequest(int id)
        {

            var createProjectRequest = await _createProjectRequestService.GetCreateProjectRequestByIdAsync(id);

            if (createProjectRequest == null)
            {
                return NotFound();
            }

            return createProjectRequest;
        }


        // PUT: api/CreateProjectRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateProjectRequest")]
        public async Task<IActionResult> PutCreateProjectRequest([FromBody] CreateProjectRequest UpdateCreateProjectRequest)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectRequestService.UpdateCreateProjectRequestAsync(UpdateCreateProjectRequest) != false ? "Update CreateProjectRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync("PutCreateProjectRequest: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateProjectRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateProjectRequest", Name = "CreateCreateProjectRequest")]
        public async Task<ActionResult<CreateProjectRequest>> PostCreateProjectRequest([FromBody] CreateProjectRequest createCreateProjectRequest)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateProjectRequest");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createProjectRequestService.CreateCreateProjectRequestAsync(createCreateProjectRequest)
                    != false ? "Create CreateProjectRequest Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync($"PostCreateProjectRequest: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateProjectRequests/5
        [HttpDelete("DeleteCreateProjectRequest/{id}")]
        public async Task<IActionResult> DeleteCreateProjectRequest(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createProjectRequestService.DeleteCreateProjectRequestAsync(id)
                    != false ? "DELETE CreateProjectRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync("PostCreateProjectRequest: Error");
            }

            return Content(statusMessage);
        }
    }
}

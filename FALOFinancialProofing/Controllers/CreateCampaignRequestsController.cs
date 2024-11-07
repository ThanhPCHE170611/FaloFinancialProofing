using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCampaignRequestsController : ControllerBase
    {
        private readonly ICreateCampaignRequestService _createCampaignRequestService;

        public CreateCampaignRequestsController(ICreateCampaignRequestService createCampaignFileService)
        {
            _createCampaignRequestService = createCampaignFileService;
        }

        //// GET: api/CreateCampaignRequests
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CreateCampaignRequest>>> GetCreateCampaignRequests()
        //{
        //    return Ok(await _createCampaignRequestService.GetAllCreateCampaignRequestsAsync());
        //}

        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpGet("GetCreateCampaignRequestByPMB")]
        public async Task<ActionResult<List<CreateCampaignRequestInformation>>> GetCreateCampaignRequests(string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            StringBuilder message = new StringBuilder();
            FilterPagingData filterPagingData = new FilterPagingData();
            List<CreateCampaignRequestInformation> data = null;
            try
            {
                data = (await _createCampaignRequestService.GetAllCreateCampaignRequestsByPMBAsync(message))
                    .ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetCreateCampaignRequests Failed!",
                        Data = data
                    });
                }
                message.Append("GetCreateCampaignRequests Successfully!");
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<CreateCampaignRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                message.Append("GetCreateCampaignRequests Failed!");
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString(),
                Data = filterPagingData
            });
        }
        // sender use this
        [HttpGet("GetAllCreateProjectRequestsByUserId/{UserId}")]
        public async Task<IActionResult> GetAllCreateProjectRequestsByUserId(string UserId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CreateCampaignRequestInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                data = (await _createCampaignRequestService.GetAllCreateCampaignRequestsByUserIdAsync(UserId, stringBuilder)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All CreateProjectRequests By UserId Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<CreateCampaignRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCreateProjectRequestsByUserId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetAllCreateProjectRequestsByUserId Successfully!",
                Data = filterPagingData
            });
        }
        // GET: api/CreateCampaignRequests/5
        [HttpGet("GetCreateCampaignRequest/{id}")]
        public async Task<ActionResult<CreateCampaignRequestInformation>> GetCreateCampaignRequest(int id)
        {

            var createCampaignRequest = await _createCampaignRequestService.GetCreateCampaignRequestByIdAsync(id);

            if (createCampaignRequest == null)
            {
                return NotFound();
            }

            return createCampaignRequest;
        }

        // PUT: api/CreateCampaignRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateCreateCampaignRequest")]
        public async Task<IActionResult> PutCreateCampaignRequest([FromBody] CreateCampaignRequest UpdateCreateCampaignRequest)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignRequestService.UpdateCreateCampaignRequestAsync(UpdateCreateCampaignRequest) != false ? "Update CreateCampaignRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync("PutCreateCampaignRequest: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/CreateCampaignRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateCreateCampaignRequest", Name = "CreateCreateCampaignRequest")]
        public async Task<ActionResult<CreateCampaignRequest>> PostCreateCampaignRequest([FromBody] CreateCampaignRequest createCreateCampaignRequest)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateCreateCampaignRequest");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _createCampaignRequestService.CreateCreateCampaignRequestAsync(createCreateCampaignRequest)
                    != false ? "Create CreateCampaignRequest Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync($"PostCreateCampaignRequest: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/CreateCampaignRequests/5
        [HttpDelete("DeleteCreateCampaignRequest/{id}")]
        public async Task<IActionResult> DeleteCreateCampaignRequest(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _createCampaignRequestService.DeleteCreateCampaignRequestAsync(id)
                    != false ? "DELETE CreateCampaignRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE CreateCampaignRequest Failed!";
                await Console.Out.WriteLineAsync("PostCreateCampaignRequest: Error");
            }

            return Content(statusMessage);
        }
    }
}

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
using System.Linq;
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

        // hiển thị toàn bộ các yêu cầu tạo dự án
        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpGet("GetCreateProjectRequestByPMB")]
        public async Task<ActionResult<List<CreateProjectRequestInformation>>> GetCreateProjectRequests(string? searchInput, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            StringBuilder message = new StringBuilder();
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            List<CreateProjectRequestInformation> data = null;
            try
            {
                data = (await _createProjectRequestService.GetAllCreateProjectRequestsByPMBAsync(message))
                    .ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All CreateProjectRequests Failed!",
                        Data = filterPagingData
                    });
                }
                message.Append("Get All CreateProjectRequests Successfully!");
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.SenderName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.ReceiverName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Title}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<CreateProjectRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                message.Append("Get All CreateProjectRequests Failed!");
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
        [RoleAttribute(AppRole.ProjectManager)]
        [HttpGet("GetAllCreateProjectRequestsByUserId/{UserId}")]
        public async Task<IActionResult> GetAllCreateProjectRequestsByUserId(string? searchInput, string UserId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CreateProjectRequestInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                data = (await _createProjectRequestService.GetAllCreateProjectRequestsByUserIdAsync(UserId, stringBuilder)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All CreateProjectRequests By UserId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.SenderName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.ReceiverName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Title}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<CreateProjectRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Get All CreateProjectRequests By UserId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All CreateProjectRequests ByUserId Successfully!",
                Data = filterPagingData
            });
        }
        // GET: api/CreateProjectRequests/5
        [HttpGet("GetCreateProjectRequest/{id}")]
        public async Task<ActionResult<CreateProjectRequestInformation>> GetCreateProjectRequest(int id)
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
                statusMessage = await _createProjectRequestService.UpdateCreateProjectRequestAsync(UpdateCreateProjectRequest) != false ? "Update CreateProjectRequest Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update CreateProjectRequest Failed!";
                await Console.Out.WriteLineAsync("PutCreateProjectRequest: Error");
            }

            return Content(statusMessage);
        }
        // Hủy đơn mình đã tạo
        [HttpPut("CancelCreateProjectRequest/{userId}/{CreateProjectRequestId}")]
        public async Task<IActionResult> CancelCreateProjectRequest(string userId, int CreateProjectRequestId)
        {
            StringBuilder statusMessage = new StringBuilder();
            bool checkValid = false;
            try
            {
                checkValid = await _createProjectRequestService.ValidateCreateProjectRequestByUserIdAndRequestIdAsync(userId, CreateProjectRequestId, statusMessage);
                if (!checkValid)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = checkValid,
                        Message = statusMessage.ToString()
                    });
                }
                checkValid = await _createProjectRequestService.CancelCreateProjectRequestAsync(userId, CreateProjectRequestId);
                if (!checkValid)
                {
                    throw new Exception("Cancel CreateProjectRequest Failed!");
                }
                statusMessage.Append("Cancel CreateProjectRequest Successfully!");
            }
            catch (Exception ex)
            {
                statusMessage.Append(ex.Message);
                await Console.Out.WriteLineAsync($"CancelCreateProjectRequest: {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Success = checkValid,
                Message = statusMessage.ToString()
            });
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

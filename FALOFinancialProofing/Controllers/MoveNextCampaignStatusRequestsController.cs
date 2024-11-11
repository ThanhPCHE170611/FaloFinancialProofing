using System.Text;
using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveNextCampaignStatusRequestsController : ControllerBase
    {
        private readonly IMoveNextCampaignStatusRequestService _moveNextCampaignStatusRequestService;

        public MoveNextCampaignStatusRequestsController(IMoveNextCampaignStatusRequestService moveNextCampaignStatusRequestService)
        {
            _moveNextCampaignStatusRequestService = moveNextCampaignStatusRequestService;
        }


        //Manh moi them
        [HttpGet("APIBASICGetAllMoveNextCampaignStatusRequests")]
        public async Task<IActionResult> GetAllMoveNextCampaignStatusRequests()
        {
            var moveNextCampaignStatusRequests = await _moveNextCampaignStatusRequestService.GetAllMoveNextCampaignStatusRequestAsync();
            if (moveNextCampaignStatusRequests == null || moveNextCampaignStatusRequests.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No MoveNextCampaignStatusRequest found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "MoveNextCampaignStatusRequests retrieved successfully.",
                Data = moveNextCampaignStatusRequests
            });
        }


        [HttpGet("APIBASICGetMoveNextCampaignStatusRequestById/{id}")]
        public async Task<IActionResult> GetMoveNextCampaignStatusRequestById(int id)
        {
            var moveNextCampaignStatusRequest = await _moveNextCampaignStatusRequestService.GetMoveNextCampaignStatusRequestByIdAsync(id);
            if (moveNextCampaignStatusRequest == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"MoveNextCampaignStatusRequest with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "MoveNextCampaignStatusRequest retrieved successfully.",
                Data = moveNextCampaignStatusRequest
            });
        }

        [HttpPost("APIBASICCreateMoveNextCampaignStatusRequest")]
        public async Task<IActionResult> CreateMoveNextCampaignStatusRequest([FromBody] CreateMoveNextCampaignStatusRequestDTO createMoveNextCampaignStatusRequestDTO)
        {
            StringBuilder stringBuilderMessage = new StringBuilder();
            var createMoveNextCampaignStatusRequest = await _moveNextCampaignStatusRequestService.CreateMoveNextCampaignStatusRequestAsync(createMoveNextCampaignStatusRequestDTO, stringBuilderMessage);
            if (createMoveNextCampaignStatusRequest == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create MoveNextCampaignStatusRequest."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "MoveNextCampaignStatusRequest created successfully.",
                Data = createMoveNextCampaignStatusRequest
            });
        }
        [RoleAttribute(AppRole.ProjectManager)]
        [HttpPost("CreateMoveNextCampaignStatusRequest")]
        public async Task<ActionResult<MoveNextCampaignStatusRequest>> CreateMoveNextCampaignStatusRequestAsync([FromBody] CreateMoveNextCampaignStatusRequestDTO requestDto)
        {

            StringBuilder stringBuilderMessage = new StringBuilder();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu DTO không hợp lệ
            }

            try
            {
                bool checkValidateProject = await _moveNextCampaignStatusRequestService.ValidateCampaignCreateAsync(requestDto, stringBuilderMessage);
                if (!checkValidateProject)
                {
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                var request = await _moveNextCampaignStatusRequestService.CreateMoveNextCampaignStatusRequestAsync(requestDto, stringBuilderMessage);
                //return CreatedAtAction(nameof(CreateMoveNextCampaignStatusRequestAsync), new { id = request.Id }, request); // Trả về kết quả
                if(request != null)
                {
                    var responseDto = _moveNextCampaignStatusRequestService.MapToDto(request);
                    return Ok(responseDto);
                }
                else
                {
                    //return BadRequest("can not create MoveNextCampaignStatusRequest");
                    return BadRequest(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RoleAttribute(AppRole.ProjectManagementBoard)]
        [HttpGet("GetAllMoveNextCampaignStatusRequestsByPMB")]
        public async Task<ActionResult<List<MoveNextCampaignStatusRequestInformation>>> GetAllMoveNextCampaignStatusRequestsByPMB(string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            StringBuilder message = new StringBuilder();
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            List<MoveNextCampaignStatusRequestInformation> data = null;
            try
            {
                data = (await _moveNextCampaignStatusRequestService.GetAllMoveNextCampaignStatusRequestsByPMBAsync(message))
                    .ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllMoveNextCampaignStatusRequestsByPMB Failed!",
                        Data = filterPagingData
                    });
                }
                message.Append("GetAllMoveNextCampaignStatusRequestsByPMB Successfully!");
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<MoveNextCampaignStatusRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                message.Append("GetAllMoveNextCampaignStatusRequestsByPMB Failed!");
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = message.ToString(),
                Data = filterPagingData
            });
        }

        [RoleAttribute(AppRole.ProjectManager)]
        [HttpGet("GetAllMoveNextCampaignStatusRequestsByUserId/{UserId}")]
        public async Task<IActionResult> GetAllMoveNextCampaignStatusRequestsByUserId(string UserId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<MoveNextCampaignStatusRequestInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                data = (await _moveNextCampaignStatusRequestService.GetAllMoveNextCampaignStatusRequestsByUserIdAsync(UserId, stringBuilder)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllMoveNextCampaignStatusRequestsByUserId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status.Equals(status));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<MoveNextCampaignStatusRequestInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllMoveNextCampaignStatusRequestsByUserId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetAllMoveNextCampaignStatusRequestsByUserId Successfully!",
                Data = filterPagingData
            });
        }

    }
}

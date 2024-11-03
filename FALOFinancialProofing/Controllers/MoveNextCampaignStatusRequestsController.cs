using System.Text;
using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.DTOs.MoveNextCampaignStatusRequestDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
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
            var createMoveNextCampaignStatusRequest = await _moveNextCampaignStatusRequestService.CreateMoveNextCampaignStatusRequestAsync(createMoveNextCampaignStatusRequestDTO);
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
                bool checkValidateProject = await _moveNextCampaignStatusRequestService.ValidateProjectCreateAsync(requestDto, stringBuilderMessage);
                if (!checkValidateProject)
                {
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                var request = await _moveNextCampaignStatusRequestService.CreateMoveNextCampaignStatusRequestAsync(requestDto);
                //return CreatedAtAction(nameof(CreateMoveNextCampaignStatusRequestAsync), new { id = request.Id }, request); // Trả về kết quả
                if(request != null)
                {
                    var responseDto = _moveNextCampaignStatusRequestService.MapToDto(request);
                    return Ok(responseDto);
                }
                else
                {
                    return BadRequest("can not create MoveNextCampaignStatusRequest");
                }
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }







        // tat tam thoi de lam theo Duc


        //[HttpPut("ApproveOrRejectRequest/{requestId}")]
        ////[Role(AppRole.ProjectManagementBoard)] // Chỉ người có vai trò ProjectManagementBoard mới truy cập được
        //public async Task<IActionResult> ApproveOrRejectRequestAsync(int requestId, [FromBody] bool isApproved)
        //{
        //    try
        //    {
        //        // Gọi service để phê duyệt hoặc từ chối
        //        bool result = await _moveNextCampaignStatusRequestService.ApproveOrRejectRequestAsync(requestId, isApproved);
        //        return result ? Ok("Request approved.") : Ok("Request rejected.");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return NotFound(ex.Message); // Trả về lỗi nếu không tìm thấy yêu cầu
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message); // Xử lý lỗi hệ thống
        //    }
        //}

    }
}

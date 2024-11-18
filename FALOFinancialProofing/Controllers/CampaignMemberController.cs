using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignMemberController : ControllerBase
    {
        private readonly ICampaignMemberService _campaignMemberService;
        private readonly AuthServices authServices;

        public CampaignMemberController(ICampaignMemberService campaignMemberService, AuthServices authServices)
        {
            _campaignMemberService = campaignMemberService;
            this.authServices = authServices;
        }

        [HttpGet("GetAllCampaignMembers")]
        public async Task<IActionResult> GetAllCampaignMembers()
        {
            var campaignMembers = await _campaignMemberService.GetAllCampaignMembersAsync();
            if (campaignMembers == null || campaignMembers.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No CampaignMembers found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMembers retrieved successfully.",
                Data = campaignMembers
            });
        }
        // có thể là tự xem chính mình hoặc admin xem uid và rid này nằm trong những campaign nào
        [HttpGet("GetAllCampaignMembersByUserIdAndRoleId")]
        public async Task<IActionResult> GetAllCampaignMembersByUserIdAndRoleId(string? searchInput, string userId, string roleId, bool? isActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignMemberInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await _campaignMemberService.GetAllCampaignMemberByUserIdAndRoleIdAsync(userId, roleId);

                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllCampaignMember By UserIdAndRoleId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.CampaignTitle}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (isActive != null)
                {
                    data = data.FindAll(x => x.IsActive == isActive);
                }
                filterPagingData.DataCount = data.Count;

                data = PaginationHelper.Paginate<CampaignMemberInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserIdAndRoleId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetAllCampaignMember By UserIdAndRoleId Successfully!",
                Data = filterPagingData
            });
        }
        // admin xem của thằng khác
        [RoleAttribute(AppRole.Admin)]
        [HttpGet("GetAllCampaignMembersByUserId")]
        public async Task<IActionResult> GetAllCampaignMembersByUserId(string? searchInput, string userId, bool? isActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignMemberInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await _campaignMemberService.GetAllCampaignMemberByUserIdAsync(userId);

                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllCampaignMember By UserId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.CampaignTitle}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (isActive != null)
                {
                    data = data.FindAll(x => x.IsActive == isActive);
                }
                filterPagingData.DataCount = data.Count;

                data = PaginationHelper.Paginate<CampaignMemberInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetAllCampaignMember By UserId Successfully!",
                Data = filterPagingData
            });
        }

        [HttpGet("GetAllCampaignMembersByCampaignId")]
        public async Task<IActionResult> GetAllCampaignMembersByCampaignId(string? searchInput, int CampaignId, bool? isActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignMemberInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            try
            {
                data = await _campaignMemberService.GetAllCampaignMemberByCampaignIdAsync(CampaignId);

                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllCampaignMembers By CampaignId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.FirstName} {x.LastName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.Email}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                if (isActive != null)
                {
                    data = data.FindAll(x => x.IsActive == isActive);
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<CampaignMemberInformation>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignMemberByUserIdAndRoleId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "GetAllCampaignMember By CampaignId Successfully!",
                Data = filterPagingData
            });
        }

        [HttpGet("GetCampaignMemberById/{id}")]
        public async Task<IActionResult> GetCampaignMemberById(int id)
        {
            var campaignMember = await _campaignMemberService.GetCampaignMemberByIdAsync(id);
            if (campaignMember == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember retrieved successfully.",
                Data = campaignMember
            });
        }

        [HttpPost("CreateCampaignMember")]
        public async Task<IActionResult> CreateCampaignMember([FromBody] CreateCampaignMemberDTO createCampaignMemberDTO)
        {
            var createCampaignMember = await _campaignMemberService.CreateCampaignMemberAsync(createCampaignMemberDTO);
            if (createCampaignMember == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create CampaignMember."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember created successfully.",
                Data = createCampaignMember
            });
        }
        [RoleAttribute(AppRole.ProjectManager, AppRole.ProjectManagementBoard, AppRole.Admin)]
        [HttpPost("CreateManyCampaignMembers/{CampaignId}/{pmUserId}")]
        public async Task<IActionResult> CreateManyCampaignMembers(int CampaignId, string pmUserId, [FromBody] List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs)
        {
            var message = new StringBuilder();

            var ValidCreateManyCampaignMemberDTOs = await _campaignMemberService.ValidateCampaignMembersCreateAsync(createManyCampaignMemberDTOs, CampaignId, pmUserId, message);
            if (ValidCreateManyCampaignMemberDTOs == null || ValidCreateManyCampaignMemberDTOs.Count == 0)
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = message.ToString()
                });
            }
            var invalidData = await _campaignMemberService.InValidCampaignMembersCreateAsync(createManyCampaignMemberDTOs, ValidCreateManyCampaignMemberDTOs);
            var InvalidUserInformation = await authServices.GetUserInformationList(invalidData);
            var CheckCreateSuccess = await _campaignMemberService.CreateManyCampaignMembersAsync(ValidCreateManyCampaignMemberDTOs, CampaignId, message);
            return Ok(new ApiResponse()
            {
                Success = CheckCreateSuccess,
                Message = message.ToString(),
                Data = InvalidUserInformation // danh sách dữ liệu không được add vào
            }
            );
        }
        [RoleAttribute(AppRole.Admin)]
        [HttpPut("UpdateCampaignMemberStatus")]
        public async Task<IActionResult> UpdateCampaignMemberStatus([FromBody] UpdateCampaignMemberStatusDTO updateCampaignMemberStatusDTO)
        {
            StringBuilder message = new StringBuilder();
            var canUpdateCampaignMember = await _campaignMemberService.UpdateCampaignMemberStatusAsync(updateCampaignMemberStatusDTO, message);
            if (!canUpdateCampaignMember)
            {
                return Ok(new
                {
                    Success = false,
                    Message = message.ToString()
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember updated successfully."
            });
        }
        [HttpPut("UpdateCampaignMember")]
        public async Task<IActionResult> UpdateCampaignMember([FromBody] UpdateCampaignMemberDTO updateCampaignMemberDTO)
        {

            var canUpdateCampaignMember = await _campaignMemberService.UpdateCampaignMemberAsync(updateCampaignMemberDTO);
            if (!canUpdateCampaignMember)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with Id = {updateCampaignMemberDTO.Id} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember updated successfully."
            });
        }
        [HttpDelete("DeleteCampaignMemberById/{id}")]
        public async Task<IActionResult> DeleteCampaignMemberById(int id)
        {
            var deleteResult = await _campaignMemberService.DeleteCampaignMemberByIdAsync(id);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CampaignMember with Id = {id} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "CampaignMember deleted successfully."
            });
        }
    }
}

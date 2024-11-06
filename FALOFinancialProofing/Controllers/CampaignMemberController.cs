using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("GetAllCampaignMember")]
        public async Task<IActionResult> GetAllCampaignMember()
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

        [HttpGet("GetAllCampaignMemberByUserIdAndRoleId")]
        public async Task<IActionResult> GetAllCampaignMemberByUserIdAndRoleId(string userId, string roleId, bool? isActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignMemberInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = await _campaignMemberService.GetAllCampaignMemberByUserIdAndRoleIdAsync(userId, roleId);

                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetAllCampaignMember By UserIdAndRoleId Failed!",
                        Data = data
                    });
                }
                if (isActive != null)
                {
                    data = data.FindAll(x => x.IsActive == isActive);
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
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

        [HttpGet("GetCampaignMemberByCampaignIdAndUserId/{id}")]
        public async Task<IActionResult> GetCampaignMemberByCampaignIdAndUserId(int id)
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
        [RoleAttribute(AppRole.ProjectManager)]
        [HttpPost("CreateManyCampaignMembers/{CampaignId}")]
        public async Task<IActionResult> CreateManyCampaignMembers(int CampaignId, [FromBody] List<CreateManyCampaignMemberDTO> createManyCampaignMemberDTOs)
        {
            var message = new StringBuilder();

            var ValidCreateManyCampaignMemberDTOs = await _campaignMemberService.ValidateCampaignMembersCreateAsync(createManyCampaignMemberDTOs, CampaignId, message);
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
        [HttpDelete("DeleteCampaignMemberByCampaignIdAndUserId/{id}")]
        public async Task<IActionResult> DeleteCampaignMemberByCampaignIdAndUserId(int id)
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

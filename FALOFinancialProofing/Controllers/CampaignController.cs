using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CreateCampaignFileServices;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
using FALOFinancialProofing.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        //private readonly ICampaignService _campaignService;
        private readonly ICampaignService _campaignService;
        private readonly ICreateCampaignRequestService _createCampaignRequestService;
        private readonly ICreateCampaignFileService _createCampaignFileService;
        private readonly RoleManager<Role> roleManager;
        private readonly ICampaignMemberService _campaignMemberService;

        public CampaignController(ICampaignService campaignService, ICreateCampaignRequestService createCampaignRequestService, ICreateCampaignFileService createCampaignFileService, RoleManager<Role> roleManager, ICampaignMemberService campaignMemberService)
        {
            _campaignService = campaignService;
            _createCampaignRequestService = createCampaignRequestService;
            _createCampaignFileService = createCampaignFileService;
            this.roleManager = roleManager;
            _campaignMemberService = campaignMemberService;
        }

        // những campaign không ở trạng thái pending và rejected
        [HttpGet("GetAllCampaignInSystem")]
        public async Task<IActionResult> GetAllCampaignInSystem(string? title, string? status, bool? IsActive, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = await _campaignService.GetAllCampaignsAsync(Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Campaign By In System Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(title))
                {
                    title = title.Trim();
                    data = data.FindAll(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    data = data.FindAll(x => x.Status == status);
                }
                if (IsActive != null)
                {
                    data = data.FindAll(x => x.IsActive == IsActive);
                }
                filterPagingData.DataCount = data.Count;
                filterPagingData.CurrentPage = currentPage;
                data = PaginationHelper.Paginate<CampaignInformation>(data.AsQueryable(), currentPage, IntConstant.PageSizeCustom).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllCampaignInSystem: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All Campaign In System Successfully!",
                Data = filterPagingData
            });
        }
        //[RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpGet("GetAllCampaignByProjectId/{ProjectId}")]// search theo title
        public async Task<IActionResult> GetAllCampaignByProjectId(string? title, int ProjectId, string? status, int currentPage = IntConstant.PageNumberDefault)
        {
            List<CampaignInformation> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            try
            {
                data = await _campaignService.GetAllCampaignsByProjectIdAsync(ProjectId, Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All Campaign By ProjectId Failed!",
                        Data = data
                    });
                }
                if (!string.IsNullOrEmpty(title))
                {
                    title = title.Trim();
                    data = data.FindAll(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
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



        [HttpGet("GetFourCampaignByFilter")]
        public async Task<IActionResult> GetFourCampaignByFilter(bool IsActive, bool OrderByAscending, int numOfElements)
        {
            List<CampaignInformation> data = null;
            try
            {
                data = await _campaignService.GetAllCampaignsAsync(Request);
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "GetFourCampaignByFilter Failed!",
                        Data = data
                    });
                }
                if (IsActive)
                {
                    data = data.FindAll(x => x.IsActive);
                }
                if (OrderByAscending)
                {
                    data = data.OrderBy(o => o.DateOfCreation).ToList();
                }
                else
                {
                    data = data.OrderByDescending(o => o.DateOfCreation).ToList();
                }
                data = data.Take(numOfElements).ToList();

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetFourCampaignByFilter: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get Four Campaign By ProjectId Successfully!",
                Data = data
            });
        }
        [HttpGet("GetCampaignDetailsById/{id}")]
        public async Task<IActionResult> GetCampaignDetailsById(int id)
        {
            var campaign = await _campaignService.GetCampaignByCampaignIdAsync(id, Request);
            if (campaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign retrieved successfully.",
                Data = campaign
            });
        }

        [RoleAttribute(AppRole.ProjectManager)]// mở nếu làm thật
        [HttpPost("CreateCampaignClient", Name = "CreateCampaignClient")]
        public async Task<ActionResult<Campaign>> PostCampaign([FromForm] CreateCampaignClientRequest createCampaignClientRequest)
        {
            // không cần thiết check modelState vì hệ thống tự động kiểm tra r
            StringBuilder stringBuilderMessage = new StringBuilder();
            try
            {
                bool checkValidateCampaign = await _campaignService.ValidateCampaignCreateAsync(createCampaignClientRequest, stringBuilderMessage);
                if (!checkValidateCampaign)
                {
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                createCampaignClientRequest.CreateBy = createCampaignClientRequest.CreateBy.Trim();
                createCampaignClientRequest.Title = createCampaignClientRequest.Title.Trim();
                createCampaignClientRequest.Description = createCampaignClientRequest.Description.Trim();
                createCampaignClientRequest.Address = createCampaignClientRequest.Address?.Trim();
                var campaign = await _campaignService.ConvertDtoToBaseClass(createCampaignClientRequest);

                var checkCampaignCreated = await _campaignService.CreateCampaignReturnEntityAsync(campaign);

                if (checkCampaignCreated == null)
                {
                    stringBuilderMessage.Append("Create campaign Failed!");
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                // tạo request trước mới tạo fileAttach**
                CreateCampaignRequest createCampaignRequest = new CreateCampaignRequest()
                {
                    SenderId = checkCampaignCreated.CreateBy,
                    CampaignId = checkCampaignCreated.Id,
                    Title = $"{checkCampaignCreated.Title}",
                    CreatedAt = DateTime.Now,
                    Status = RequestStatus.Pending,
                    Description = $"{createCampaignClientRequest.Description}",
                };
                var CreateCampaignRequestCreated = await _createCampaignRequestService.CreateCreateCampaignRequestReturnEntityAsync(createCampaignRequest);
                if (CreateCampaignRequestCreated == null)
                {
                    stringBuilderMessage.Append("Create Campaign Request Failed!");
                    return Ok(new
                    {
                        Message = stringBuilderMessage.ToString()
                    });
                }
                // Tạo fileYêu cầu có thể có file hoặc không
                if (createCampaignClientRequest.FormFiles != null)
                {
                    var createCampaignFiles = await _createCampaignFileService.SaveUploadedFilesAsync(createCampaignClientRequest.FormFiles, CreateCampaignRequestCreated.Id);
                    bool checkCreate = await _createCampaignFileService.CreateCreateCampaignFilesAsync(createCampaignFiles);
                    if (checkCreate == false)
                    {
                        stringBuilderMessage.Append("Create Campaign Request files Failed!");
                        return Ok(new
                        {
                            Message = stringBuilderMessage.ToString()
                        });
                    }
                    //}
                }
                stringBuilderMessage.Append("Create Campaign Successfully!");
            }
            catch (Exception ex)
            {
                stringBuilderMessage.Append("Create Campaign Failed!");
                await Console.Out.WriteLineAsync($"PostCampaign: {ex.Message}");
            }

            return Content(stringBuilderMessage.ToString());
        }
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.ProjectManager, AppRole.Admin)]
        [HttpPut("UpdateCampaign")]
        public async Task<IActionResult> UpdateCampaign([FromForm] UpdateCampaignDTO updateCampaignDTO)
        {
            StringBuilder message = new StringBuilder();
            bool checkValid = false;
            try
            {
                checkValid = await _campaignService.ValidateCampaignUpdateAsync(updateCampaignDTO, message);
                if (!checkValid)
                {
                    return Ok(new ApiResponse()
                    {
                        Message = message.ToString(),
                        Success = checkValid
                    });
                }
                updateCampaignDTO.Title = updateCampaignDTO.Title.Trim();
                updateCampaignDTO.Description = updateCampaignDTO.Description.Trim();
                updateCampaignDTO.Address = updateCampaignDTO.Address?.Trim();
                updateCampaignDTO.Status = updateCampaignDTO.Status?.Trim();
                checkValid = await _campaignService.UpdateCampaignAsync(updateCampaignDTO, message);
                if (checkValid)
                    message.Append("Campaign updated successfully!");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateCampaign: Error {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Message = message.ToString(),
                Success = checkValid
            });
        }
        [RoleAttribute(AppRole.ProjectManagementBoard, AppRole.Admin)]
        [HttpDelete("DeleteCampaignById/{id}")]
        public async Task<IActionResult> DeleteCampaignById(int id)
        {
            var deleteResult = await _campaignService.DeleteCampaignByIdAsync(id);
            if (!deleteResult)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {id} not found or could not be deleted."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign deleted successfully."
            });
        }

        [HttpGet("GetAllCampaignByUserAttendIn/{userId}")]
        public async Task<IActionResult> GetAllCampaignByUserIdAndRole(string userId, string currentRole)
        {
            var campaigns = await _campaignService.GetAllCampaignByUserIdAndRoleAsync(userId, currentRole);
            if (campaigns == null || campaigns.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Campaigns found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaigns retrieved successfully.",
                Data = campaigns
            });
        }


        [HttpPost("updateenddateforprojectmanager")]
        public async Task<IActionResult> UpdateEndDateForProjectManager(int campaignId, string userId, string currentRole, DateTime newDateTime)
        {
            var message = new StringBuilder();
            var updateEndDateCamapaign = await _campaignService.UpdateEndDateForProjectManagerAsync(campaignId, userId, currentRole, newDateTime, message);
            if (updateEndDateCamapaign == null)
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
                Message = "Update End Date for Project Manager successfully.",
                Data = $"New DateTime: {newDateTime}"
            });
        }
    }
}

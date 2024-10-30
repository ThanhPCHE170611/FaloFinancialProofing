using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.DTOs.CampaignDTO;
using FALOFinancialProofing.DTOs.CreateProjectRequestDTO;
using FALOFinancialProofing.DTOs.ProjectDTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CreateCampaignFileServices;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
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

        public CampaignController(ICampaignService campaignService, ICreateCampaignRequestService createCampaignRequestService, ICreateCampaignFileService createCampaignFileService)
        {
            _campaignService = campaignService;
            _createCampaignRequestService = createCampaignRequestService;
            _createCampaignFileService = createCampaignFileService;
        }

        //[HttpGet("GetAllCampaign")]
        //public async Task<IActionResult> GetAllCampaign()
        //{
        //    var campaigns = await _campaignService.GetAllCampaignsAsync();
        //    if (campaigns == null || campaigns.Count == 0)
        //    {
        //        return Ok(new
        //        {
        //            Success = false,
        //            Message = "No Campaigns found."
        //        });
        //    }

        //    return Ok(new
        //    {
        //        Success = true,
        //        Message = "Campaigns retrieved successfully.",
        //        Data = campaigns
        //    });
        //}
        [HttpGet("GetAllCampaign")]
        public async Task<IActionResult> GetAllCampaign()
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync();
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

        [HttpGet("GetCampaignById/{id}")]
        public async Task<IActionResult> GetCampaignById(int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
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

        [HttpPost("CreateCampaign")]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDTO createCampaignDTO)
        {
            var createCampaign = await _campaignService.CreateCampaignAsync(createCampaignDTO);
            if (createCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Unable to create Campaign."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign created successfully.",
                Data = createCampaign
            });
        }


        [RoleAttribute(AppRole.ProjectManager)] // mở nếu làm thật
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
                // tạo request trước mới tạo fileAttach
                CreateCampaignRequest createCampaignRequest = new CreateCampaignRequest()
                {
                    SenderId = checkCampaignCreated.CreateBy,
                    CampaignId = checkCampaignCreated.Id,
                    Title = "Create Campaign",
                    CreatedAt = DateTime.Now,
                    Status = RequestStatus.Pending
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
                //statusMessage = await _projectService.CreateProjectAsync(createProject)
                //    != false ? "Create Project Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                stringBuilderMessage.Append("Create Campaign Failed!");
                await Console.Out.WriteLineAsync($"PostCampaign: {ex.Message}");
            }

            return Content(stringBuilderMessage.ToString());
        }
        [HttpPut("UpdateCampaign")]
        public async Task<IActionResult> UpdateCampaign([FromBody] UpdateCampaignDTO updateCampaignDTO)
        {

            var canUpdateCampaign = await _campaignService.UpdateCampaignAsync(updateCampaignDTO);
            if (!canUpdateCampaign)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Campaign with Id = {updateCampaignDTO.Id} not found or could not be updated."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Campaign updated successfully."
            });
        }
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


    }
}

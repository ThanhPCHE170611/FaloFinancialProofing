using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Extensions;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using FALOFinancialProofing.Services.RequestFormServices;
using FALOFinancialProofing.Services.VoucherServices;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestFormController : ControllerBase
    {
        private readonly IRequestFormServices requestFormService;
        private readonly IAttachmentFileServices attachmentFileService;
        private readonly IApproveProcessServices approveProcessServices;
        private readonly IVoucherServices voucherServices;

        public RequestFormController(IRequestFormServices requestFormService, 
            IAttachmentFileServices attachmentFileService, 
            IApproveProcessServices approveProcessServices,
            IVoucherServices voucherServices
            )
        {
            this.requestFormService = requestFormService;
            this.attachmentFileService = attachmentFileService;
            this.approveProcessServices = approveProcessServices;
            this.voucherServices = voucherServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequestForms()
        {
            var requestForms = await requestFormService.GetAllRequestFormsAsync();
            if (requestForms == null || requestForms.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No RequestForms found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "RequestForms retrieved successfully.",
                Data = requestForms
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestFormById(int id)
        {
            var requestForm = await requestFormService.GetRequestFormByIdAsync(id);
            if (requestForm == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Request Form with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = requestForm
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRequestFormsByUserId(string userId)
        {
            var requestForm = await requestFormService.GetRequestFormByUserIdAsync(userId);
            if (requestForm == null || requestForm.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Request Form Create By User = {userId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = requestForm
            });
        }

        [HttpPost("creatnewprepayrequest")]
        public async Task<IActionResult> CreateNewPrePayRequestForm([FromForm] CreateFormRequest requestFormRequest)
        {
            StringBuilder message = new StringBuilder();
            // Validate Data from RequestForm
            var validatedRequest = await requestFormService.ValidateRequestForm(requestFormRequest, message);
            if ((bool)!validatedRequest.IsValidate)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Create Request Failed {message}"  
                });
            }
            // Create new RequestForm
            RequestFormInformation newRequestFormInfor = new RequestFormInformation
            {
                CreateAt = validatedRequest.CreateAt,
                Description = validatedRequest.Description,
                ExpectedMoney = validatedRequest.ExpectedMoney,
                Status = Resource.ProcessStatus,
                CreatedBy = validatedRequest.CreatedBy,
                CampaignId = StringExtension.ParseStringToInt(validatedRequest.CampaignId),
                TypeId = 1
            };
            var newRequestForm = await requestFormService.CreateRequestFormAsync(newRequestFormInfor);
            if(newRequestForm == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Create new RequestForm failed."
                });
            }
            //prepay request -> type = 1
            // Create new Approve Process
            var approveProcessDTO = new ApproveProcessRequest
            {
                ApproveNumber = 1,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = newRequestForm.Id,
                ApproverId = requestFormRequest.ApproverId
            };
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(approveProcessDTO);
            //Create new AttachmentFile
            var attachmentFiles = await requestFormService.SaveUploadedFilesAsync(validatedRequest.UploadFiles, newRequestForm.Id);
            var canCreateAttachmentFiles = await attachmentFileService.CreateManyAttachmentFileAsync(attachmentFiles);
            if(!canCreateAttachmentFiles)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Create new AttachmentFiles failed."
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Create new PrePay RequestForm successfully.",
                Data = new {RequestId = newRequestForm.Id, 
                ApproveProcessId = newApproveProcess.Id }
            });
        }

        [HttpGet("getapproverlistforvolunteer/{campaignId}")]
        public async Task<IActionResult> GetApproverListForVolunteer(int campaignId)
        {
            var approverListAsync = await requestFormService.GetApproverListForVolunteer(campaignId);
            if(approverListAsync == null || approverListAsync.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Approver found."
                });
            } 
            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = approverListAsync
            });
            
        }
        [HttpGet("getapproverlistforvolunteerleader/{campaignId}")]
        public async Task<IActionResult> GetApproverListForVolunteerLeader(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForVolunteerLeader(campaignId);
            if(approverAsync == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Approver found."
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = approverAsync
            });
        }
        
        [HttpGet("getapproverlistforaccounting/{campaignId}")]
        public async Task<IActionResult> GetApproverListForAccounting(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForAccounting(campaignId);
            if(approverAsync == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Approver found."
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = approverAsync
            });
        }
        
        [HttpGet("getapproverlistforprojectmanagement/{campaignId}")]
        public async Task<IActionResult> GetApproverListForProjectManagement(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForProjectManagement(campaignId);
            if(approverAsync == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No Approver found."
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = approverAsync
            });
        }
        
        [HttpPost("uploadvoucherforaccounting/{approveId}")]
        public async Task<IActionResult> UploadVoucherForAccounting(int approveId, List<IFormFile> files)
        {
            var vouchers = await requestFormService.SaveUploadedVoucherAsync(approveId, files);
            var canCreateVouchers = await voucherServices.CreateManyVoucherAsync(vouchers);
            if (!canCreateVouchers)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Create new Voucher failed."
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Create new PrePay RequestForm successfully.",
                Data = vouchers
            });
        }
        
    }
}

using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Extensions;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using FALOFinancialProofing.Services.RequestFormServices;
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

        public RequestFormController(IRequestFormServices requestFormService, IAttachmentFileServices attachmentFileService, IApproveProcessServices approveProcessServices)
        {
            this.requestFormService = requestFormService;
            this.attachmentFileService = attachmentFileService;
            this.approveProcessServices = approveProcessServices;
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
                Status = "Pending",
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
                Data = newRequestFormInfor
            });
        }
    }
}

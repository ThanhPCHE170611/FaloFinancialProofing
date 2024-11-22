using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Extensions;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using FALOFinancialProofing.Services.CampaignMemberService;
using FALOFinancialProofing.Services.RequestFormServices;
using FALOFinancialProofing.Services.VoucherServices;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestFormController : ControllerBase
    {
        private readonly IRequestFormServices requestFormService;
        private readonly IAttachmentFileServices attachmentFileService;
        private readonly IApproveProcessServices approveProcessServices;
        private readonly IVoucherServices voucherServices;
        private readonly ICampaignMemberService campaignMemberServices;

        public RequestFormController(IRequestFormServices requestFormService, 
            IAttachmentFileServices attachmentFileService, 
            IApproveProcessServices approveProcessServices, 
            IVoucherServices voucherServices, 
            ICampaignMemberService campaignMemberServices)
        {
            this.requestFormService = requestFormService;
            this.attachmentFileService = attachmentFileService;
            this.approveProcessServices = approveProcessServices;
            this.voucherServices = voucherServices;
            this.campaignMemberServices = campaignMemberServices;
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
            requestFormRequest.TypeId = IntConstant.PrePayRequestType.ToString();
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
            var attachmentfiles = new List<IFormFile>();
            var voucherFiles = new List<IFormFile>();
            if(requestFormRequest.UploadFiles != null)
            {
                string attachmentfileExtension = Path.GetExtension(requestFormRequest.UploadFiles.FileName);

                if (!string.Equals(attachmentfileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    message.Append("Attachment file must be a zip file");
                    return Ok(new
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                attachmentfiles.Add(requestFormRequest.UploadFiles);
            }
            if(requestFormRequest.VoucherFile != null)
            {
                string voucherfileExtension = Path.GetExtension(requestFormRequest.VoucherFile.FileName);

                if (!string.Equals(voucherfileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    message.Append("Voucher file must be a zip file");
                    return Ok(new
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                voucherFiles.Add(requestFormRequest.VoucherFile);
            }
            // Create new RequestForm
            var newRequestFormInfor = new RequestFormInformation
            {
                CreateAt = validatedRequest.CreateAt,
                Description = validatedRequest.Description,
                ExpectedMoney = validatedRequest.ExpectedMoney,
                Status = Resource.ProcessStatus,
                CreatedBy = validatedRequest.CreatedBy,
                CampaignId = StringExtension.ParseStringToInt(validatedRequest.CampaignId),
                TypeId = IntConstant.PrePayRequestType
            };
            var newRequestForm = await requestFormService.CreateRequestFormAsync(newRequestFormInfor);
            if (newRequestForm == null)
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
                ApproveNumber = IntConstant.FirstApproveNumber,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = newRequestForm.Id,
                ApproverId = requestFormRequest.ApproverId
            };
            // special case for volunteer leader select approver is still they
            if(requestFormRequest.ApproverId == requestFormRequest.CreatedBy)
            {
                approveProcessDTO.ApproveStatus = Resource.ApprovedStatus;
                // create and accept for leader
                var newApproveProcessForLeader = await approveProcessServices.CreateApproveProcessAsync(approveProcessDTO);
                //Create new AttachmentFile
                if (requestFormRequest.UploadFiles != null)
                {
                    var attachmentFiles = await requestFormService.SaveAttachmentFilesAsync(attachmentfiles, newRequestForm.Id, newRequestForm.TypeId);
                    var canCreateAttachmentFiles = await attachmentFileService.CreateManyAttachmentFileAsync(attachmentFiles);
                    if (!canCreateAttachmentFiles)
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Create new AttachmentFiles failed."
                        });
                    }
                }
                if (requestFormRequest.VoucherFile != null)
                {
                    var newVouchers = await requestFormService.SaveUploadedVoucherAsync(newApproveProcessForLeader.Id, voucherFiles);
                    var canCreateVouchers = await voucherServices.CreateManyVoucherAsync(newVouchers);
                    if (!canCreateVouchers)
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Create new Voucher failed."
                        });
                    }
                }
                // pushhing approve process for accounting
                var accountingInCampaign = await requestFormService.GetApproverForVolunteerLeader(Int32.Parse(requestFormRequest.CampaignId));
                var approveProcessForAccounting = new ApproveProcessRequest
                {
                    ApproveNumber = IntConstant.SecondApproveNumber,
                    ApproveStatus = Resource.ProcessStatus,
                    RequestId = newRequestForm.Id,
                    ApproverId = accountingInCampaign.UserId
                };
                var newApproveProcessForAccounting = await approveProcessServices.CreateApproveProcessAsync(approveProcessForAccounting);
                
                return Ok(new
                {
                    Success = true,
                    Message = "Create new PrePay RequestForm successfully.",
                    Data = new
                    {
                        RequestId = newRequestForm.Id,
                        ApproveProcessId = newApproveProcessForAccounting.Id
                    }
                });
            }
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(approveProcessDTO);
            //Create new AttachmentFile
            if (requestFormRequest.UploadFiles != null)
            {
                var attachmentFiles = await requestFormService.SaveAttachmentFilesAsync(attachmentfiles, newRequestForm.Id, newRequestForm.TypeId);
                var canCreateAttachmentFiles = await attachmentFileService.CreateManyAttachmentFileAsync(attachmentFiles);
                if (!canCreateAttachmentFiles)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Create new AttachmentFiles failed."
                    });
                }
            }
            if (requestFormRequest.VoucherFile != null)
            {
                var newVouchers = await requestFormService.SaveUploadedVoucherAsync(newApproveProcess.Id, voucherFiles);
                var canCreateVouchers = await voucherServices.CreateManyVoucherAsync(newVouchers);
                if (!canCreateVouchers)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Create new Voucher failed."
                    });
                }
            }

            return Ok(new
            {
                Success = true,
                Message = "Create new PrePay RequestForm successfully.",
                Data = new
                {
                    RequestId = newRequestForm.Id,
                    ApproveProcessId = newApproveProcess.Id
                }
            });
        }

        [HttpPost("createnewpaymentrequest")]
        public async Task<IActionResult> CreateNewPaymentRequestForm([FromForm] CreateFormRequest requestFormRequest)
        {
            requestFormRequest.TypeId = IntConstant.PaymentRequestType.ToString();
            StringBuilder message = new StringBuilder();
            // Validate Data from RequestForm
            var attachmentfiles = new List<IFormFile>();
            var voucherFiles = new List<IFormFile>();
            if (requestFormRequest.UploadFiles != null)
            {
                string attachmentfileExtension = Path.GetExtension(requestFormRequest.UploadFiles.FileName);

                if (!string.Equals(attachmentfileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    message.Append("Attachment file must be a zip file");
                    return Ok(new
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                attachmentfiles.Add(requestFormRequest.UploadFiles);
            }
            if (requestFormRequest.VoucherFile != null)
            {
                string voucherfileExtension = Path.GetExtension(requestFormRequest.VoucherFile.FileName);

                if (!string.Equals(voucherfileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    message.Append("Voucher file must be a zip file");
                    return Ok(new
                    {
                        Success = false,
                        Message = message.ToString()
                    });
                }
                voucherFiles.Add(requestFormRequest.VoucherFile);
            }
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
            var newRequestFormInfor = new RequestFormInformation
            {
                CreateAt = validatedRequest.CreateAt,
                Description = validatedRequest.Description,
                ExpectedMoney = validatedRequest.ExpectedMoney,
                Status = Resource.ProcessStatus,
                CreatedBy = validatedRequest.CreatedBy,
                CampaignId = StringExtension.ParseStringToInt(validatedRequest.CampaignId),
                TypeId = IntConstant.PaymentRequestType
            };
            var newRequestForm = await requestFormService.CreateRequestFormAsync(newRequestFormInfor);
            if (newRequestForm == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Create new RequestForm failed."
                });
            }
            // Create new Approve Process
            var approveProcessDTO = new ApproveProcessRequest
            {
                ApproveNumber = IntConstant.FirstApproveNumber,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = newRequestForm.Id,
                ApproverId = requestFormRequest.ApproverId
            };
            if (requestFormRequest.ApproverId == requestFormRequest.CreatedBy)
            {
                approveProcessDTO.ApproveStatus = Resource.ApprovedStatus;
                // create and accept for leader
                var newApproveProcessForLeader = await approveProcessServices.CreateApproveProcessAsync(approveProcessDTO);
                //Create new AttachmentFile
                if (requestFormRequest.UploadFiles != null)
                {
                    var attachmentFiles = await requestFormService.SaveAttachmentFilesAsync(attachmentfiles, newRequestForm.Id, newRequestForm.TypeId);
                    var canCreateAttachmentFiles = await attachmentFileService.CreateManyAttachmentFileAsync(attachmentFiles);
                    if (!canCreateAttachmentFiles)
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Create new AttachmentFiles failed."
                        });
                    }
                }
                if (requestFormRequest.VoucherFile != null)
                {
                    var newVouchers = await requestFormService.SaveUploadedVoucherAsync(newApproveProcessForLeader.Id, voucherFiles);
                    var canCreateVouchers = await voucherServices.CreateManyVoucherAsync(newVouchers);
                    if (!canCreateVouchers)
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Create new Voucher failed."
                        });
                    }
                }
                // pushhing approve process for accounting
                var accountingInCampaign = await requestFormService.GetApproverForVolunteerLeader(Int32.Parse(requestFormRequest.CampaignId));
                var approveProcessForAccounting = new ApproveProcessRequest
                {
                    ApproveNumber = IntConstant.SecondApproveNumber,
                    ApproveStatus = Resource.ProcessStatus,
                    RequestId = newRequestForm.Id,
                    ApproverId = accountingInCampaign.UserId
                };
                var newApproveProcessForAccounting = await approveProcessServices.CreateApproveProcessAsync(approveProcessForAccounting);

                return Ok(new
                {
                    Success = true,
                    Message = "Create new Payment RequestForm successfully.",
                    Data = new
                    {
                        RequestId = newRequestForm.Id,
                        ApproveProcessId = newApproveProcessForAccounting.Id
                    }
                });
            }
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(approveProcessDTO);
            //Create new AttachmentFile
            if (requestFormRequest.UploadFiles != null)
            {
                var attachmentFiles = await requestFormService.SaveAttachmentFilesAsync(attachmentfiles, newRequestForm.Id, newRequestForm.TypeId);
                var canCreateAttachmentFiles = await attachmentFileService.CreateManyAttachmentFileAsync(attachmentFiles);
                if (!canCreateAttachmentFiles)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Create new AttachmentFiles failed."
                    });
                }
            }
            if (requestFormRequest.VoucherFile != null)
            {
                var newVouchers = await requestFormService.SaveUploadedVoucherAsync(newApproveProcess.Id, voucherFiles);
                var canCreateVouchers = await voucherServices.CreateManyVoucherAsync(newVouchers);
                if (!canCreateVouchers)
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Create new Voucher failed."
                    });
                }
            }

            return Ok(new
            {
                Success = true,
                Message = "Create new Payment RequestForm successfully.",
                Data = new
                {
                    RequestId = newRequestForm.Id,
                    ApproveProcessId = newApproveProcess.Id
                }
            });
        }

        [HttpGet("getapproverlistforvolunteer/{campaignId}")]
        public async Task<IActionResult> GetApproverListForVolunteer(int campaignId)
        {
            var approverListAsync = await requestFormService.GetApproverListForVolunteer(campaignId);
            if (approverListAsync == null || approverListAsync.Count == 0)
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
        [HttpGet("getapproverforvolunteerleader/{campaignId}")]
        public async Task<IActionResult> GetApproverForVolunteerLeader(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForVolunteerLeader(campaignId);
            if (approverAsync == null)
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

        [HttpGet("getapproverforaccounting/{campaignId}")]
        public async Task<IActionResult> GetApproverForAccounting(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForAccounting(campaignId);
            if (approverAsync == null)
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

        [HttpGet("getapproverforprojectmanagement/{campaignId}")]
        public async Task<IActionResult> GetApproverForProjectManagement(int campaignId)
        {
            var approverAsync = await requestFormService.GetApproverForProjectManagement(campaignId);
            if (approverAsync == null)
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

        [HttpGet("getallprepayrequestincampaign/{campaignId}")]
        public async Task<IActionResult> GetAllPrePayRequestInCampaign(int campaignId, string userId,
            string? status,
            int page = IntConstant.PageNumberDefault)
        {
            var requestForms = await requestFormService.GetAllPrePayRequestInCampaign(campaignId, userId);
            if (requestForms == null || requestForms.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No RequestForms found."
                });
            }

            var filteredResult = requestForms.AsEnumerable();
            if (!string.IsNullOrEmpty(status))
            {
                filteredResult = filteredResult.Where(x => x.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            var totalRecords = filteredResult.Count();
            var pagedResult = filteredResult
            .Skip((page - 1) * IntConstant.PageSize)
                .Take(IntConstant.PageSize)
                .ToList();

            var response = new
            {
                TotalRecords = totalRecords,
                Page = page,
                Data = pagedResult
            };
            return Ok(new
            {
                Success = true,
                Message = "RequestForms retrieved successfully.",
                Data = response
            });
        }

        [HttpGet("getrequestdetailbyrequestid/{requestId}")]
        public async Task<IActionResult> GetPrePayRequestDetailByRequestId(int requestId)
        {
            var requestForm = await requestFormService.GetRequestDetailByRequestId(requestId);
            if (requestForm == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Request Form with Id = {requestId} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request Form retrieved successfully.",
                Data = requestForm
            });
        }

        [HttpGet("getallpaymentrequestincampaign/{campaignId}")]
        public async Task<IActionResult> GetAllPaymentRequestInCampaign(int campaignId, string userId,
            string? status,
            int page = IntConstant.PageNumberDefault)
        {
            var requestForms = await requestFormService.GetAllPaymentRequestInCampaign(campaignId, userId);
            if (requestForms == null || requestForms.Count == 0)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "No RequestForms found."
                });
            }
            var filteredResult = requestForms.AsEnumerable();
            if (!string.IsNullOrEmpty(status))
            {
                filteredResult = filteredResult.Where(x => x.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            var totalRecords = filteredResult.Count();
            var pagedResult = filteredResult
            .Skip((page - 1) * IntConstant.PageSize)
                .Take(IntConstant.PageSize)
                .ToList();

            var response = new
            {
                TotalRecords = totalRecords,
                Page = page,
                Data = pagedResult
            };
            return Ok(new
            {
                Success = true,
                Message = "RequestForms retrieved successfully.",
                Data = response
            });
        }

        [HttpGet("cancelrequest/{requestId}")]
        public async Task<IActionResult> CancelRequest(int requestId)
        {
            var message = new StringBuilder();
            var requestFormIsCancel = await requestFormService.CancelRequest(requestId, message);
            if (requestFormIsCancel == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Request Form can't not be cancel " + message.ToString()
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request Form canceled successfully.",
                Data = new {
                    Id = requestFormIsCancel.Id,
                    Description = requestFormIsCancel.Description,
                    Status = requestFormIsCancel.Status,
                    ExpectedMoney = requestFormIsCancel.ExpectedMoney
                }
            });
        }

        [HttpPost("addmissingattachmentforrequest/{requestId}")]
        public async Task<IActionResult> AddMissingAttachmentFileForRequest(int requestId, IFormFile attachment)
        {
            var message = new StringBuilder();
            string fileExtension = Path.GetExtension(attachment.FileName);

            if (!string.Equals(fileExtension, ".zip", StringComparison.OrdinalIgnoreCase))
            {
                message.Append("Attachment file must be a zip file");
                return Ok(new
                {
                    Success = false,
                    Message = message.ToString()
                });
            }
            var addMissingFileSuccess = await requestFormService.AddMissingAttachmentFileForRequestAsync(requestId, message, attachment);
            if(!addMissingFileSuccess)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Add missing attachment file failed. {message}"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = $"Add missing attachment file success"
            });
        }
    }
}

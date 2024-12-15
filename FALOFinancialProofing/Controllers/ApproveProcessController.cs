using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.DTOs.CampaignMemberDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.RequestFormServices;
using FALOFinancialProofing.Services.VoucherServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApproveProcessController : ControllerBase
    {
        private readonly IApproveProcessServices approveProcessServices;
        private readonly IRequestFormServices requestFormServices;
        private readonly FALOFinancialProofingDbContext _dbContext;
        private readonly ICampaignMemberService campaignMemberService;

        public ApproveProcessController(IApproveProcessServices approveProcessServices, IRequestFormServices requestFormServices, 
            FALOFinancialProofingDbContext dbContext, ICampaignMemberService campaignMemberService)
        {
            this.approveProcessServices = approveProcessServices;
            this.requestFormServices = requestFormServices;
            _dbContext = dbContext;
            this.campaignMemberService = campaignMemberService;
        }

        [HttpGet("getallprepayrequestforvolunteerleaderincampaign/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForVolunteerLeader(string userid, string currentLoggingRole,
            int campaignId,
            string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForVolunteerLeader(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for volunteer leader successfully",
                    Data = responses
                });
            }
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
                Message = "Get all prepay request for volunteer leader successfully",
                Data = response
            });
        }

        [HttpGet("getallpaymentrequestforvolunteerleaderincampaign/{userid}")]
        public async Task<IActionResult> GetAllPaymentRequestForVolunteerLeader(string userid, string currentLoggingRole, int campaignId,
            string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPaymentRequestForVolunteerLeader(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for volunteer leader successfully",
                    Data = responses
                });
            }
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
                Message = "Get all payment request for volunteer leader successfully",
                Data = response
            });
        }

        [HttpGet("getallprepayrequestforaccountingincampaign/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForAccounting(string userid, string currentLoggingRole, int campaignId,
             string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForAccounting(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for accounting successfully",
                    Data = responses
                });
            }
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
                Message = "Get all prepay request for accounting successfully",
                Data = response
            });
        }
        [HttpGet("getallpaymentrequestforaccountingincampaign/{userid}")]
        public async Task<IActionResult> GetAllPaymentRequestForAccounting(string userid, string currentLoggingRole, int campaignId,
             string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPaymentRequestForAccounting(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for accounting successfully",
                    Data = responses
                });
            }
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
                Message = "Get all payment request for accounting successfully",
                Data = response
            });
        }

        [HttpGet("getallprepayrequestforprojectmanagerincampaign/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForProjectManager(string userid, string currentLoggingRole, int campaignId,
             string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForProjectManager(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for project manager successfully",
                    Data = responses
                });
            }
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
                Message = "Get all prepay request for project manager successfully",
                Data = response
            });
        }

        [HttpGet("getallpaymentrequestforprojectmanagerincampaign/{userid}")]
        public async Task<IActionResult> GetAllPaymentRequestForProjectManager(string userid, string currentLoggingRole, int campaignId,
             string? status = null,
            string? createdByEmail = null,
            int page = IntConstant.PageNumberDefault)
        {
            var result = await approveProcessServices.GetAllPaymentRequestForProjectManager(userid, currentLoggingRole);
            var filteredResult = result.Where(x => x.CampaignId == campaignId);
            if (!string.IsNullOrEmpty(createdByEmail))
            {
                filteredResult = filteredResult.Where(x => x.CreateByEmail.ToLower().Equals(createdByEmail.ToLower()));
                var totalRecord = filteredResult.Count();
                var pagedResults = filteredResult
                .Skip((page - 1) * IntConstant.PageSize)
                    .Take(IntConstant.PageSize)
                    .ToList();

                var responses = new
                {
                    TotalRecords = totalRecord,
                    Page = page,
                    Data = pagedResults
                };
                return Ok(new
                {
                    Success = true,
                    Message = "Get all prepay request for project manager successfully",
                    Data = responses
                });
            }
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
                Message = "Get all payment request for project manager successfully",
                Data = response
            });
        }

        [HttpPost("rejectprepayrequestforvolunteerleader")]
        public async Task<IActionResult> RejectPrePayRequestForVolunteerLeader([FromBody] RejectRequest rejectRequest)
        {
            var message = new StringBuilder();
            var feedBackStringBuilder = new StringBuilder();
            var canReject = await approveProcessServices.RejectPrePayRequestForLeader(rejectRequest.UserId, rejectRequest.CurrentLoggingRole, rejectRequest.RequestId, message, rejectRequest.Feedback, feedBackStringBuilder);
            if (!canReject)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Reject action cannot be done " + message.ToString()
                });
            }
            // update request form status to reject
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(rejectRequest.RequestId);
            var requestFormDTO = new RequestFormDTO
            {
                Id = requestForm.Id,
                CreateAt = requestForm.CreateAt,
                Description = requestForm.Description,
                ExpectedMoney = requestForm.ExpectedMoney,
                CreatedBy = requestForm.CreatedBy,
                CampaignId = requestForm.CampaignId,
                TypeId = requestForm.TypeId,
                Status = Resource.RejectedStatus,
                FeedBack = feedBackStringBuilder.ToString()
            };
            var canUpdatedRequest = await requestFormServices.UpdateRequestFormAsync(requestFormDTO);
            if (canUpdatedRequest == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Cannot update Request Form"
                });
            }

            // update approve process status to reject
            var approveProcess = await approveProcessServices.GetApproveProcessesByRequestIdAndApproveIdAsync(rejectRequest.RequestId, rejectRequest.UserId);
            var updateApproveProcessDTO = new ApproveProcessRequest
            {
                Id = approveProcess.Id,
                ApproveNumber = approveProcess.ApproveNumber,
                ApproveStatus = Resource.RejectedStatus,
                RequestId = approveProcess.RequestId,
                ApproverId = approveProcess.ApproverId,
            };
            var canUpdatedApproveProcess = await approveProcessServices.UpdateApproveProcessAsync(updateApproveProcessDTO);
            return Ok(new
            {
                Success = true,
                Message = $"Update Request form with id = {requestForm.Id} to reject status"
            });
        }

        [HttpPost("rejectprepayrequestforaccounting")]
        public async Task<IActionResult> RejectPrePayRequestForAccounting([FromBody] RejectRequest rejectRequest)
        {
            var message = new StringBuilder();
            var feedBackStringBuilder = new StringBuilder();
            var canReject = await approveProcessServices.RejectPrePayRequestForAccounting(rejectRequest.UserId, rejectRequest.CurrentLoggingRole, rejectRequest.RequestId, message, rejectRequest.Feedback, feedBackStringBuilder);
            if (!canReject)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Reject action cannot be done " + message.ToString()
                });
            }
            // update request form status to reject
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(rejectRequest.RequestId);
            var requestFormDTO = new RequestFormDTO
            {
                Id = requestForm.Id,
                CreateAt = requestForm.CreateAt,
                Description = requestForm.Description,
                ExpectedMoney = requestForm.ExpectedMoney,
                CreatedBy = requestForm.CreatedBy,
                CampaignId = requestForm.CampaignId,
                TypeId = requestForm.TypeId,
                Status = Resource.RejectedStatus,
                FeedBack = feedBackStringBuilder.ToString()
            };
            var canUpdatedRequest = await requestFormServices.UpdateRequestFormAsync(requestFormDTO);
            if (canUpdatedRequest == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Cannot update Request Form"
                });
            }

            // update approve process status to reject
            var approveProcess = await approveProcessServices.GetApproveProcessesByRequestIdAndApproveIdAsync(rejectRequest.RequestId, rejectRequest.UserId);
            var updateApproveProcessDTO = new ApproveProcessRequest
            {
                Id = approveProcess.Id,
                ApproveNumber = approveProcess.ApproveNumber,
                ApproveStatus = Resource.RejectedStatus,
                RequestId = approveProcess.RequestId,
                ApproverId = approveProcess.ApproverId,
            };
            var canUpdatedApproveProcess = await approveProcessServices.UpdateApproveProcessAsync(updateApproveProcessDTO);
            return Ok(new
            {
                Success = true,
                Message = $"Update Request form with id = {requestForm.Id} to reject status"
            });
        }

        [HttpPost("rejectprepayrequestforprojectmanager")]
        public async Task<IActionResult> RejectPrePayRequestForProjectManager([FromBody] RejectRequest rejectRequest)
        {
            var message = new StringBuilder();
            var feedBackStringBuilder = new StringBuilder();
            var canReject = await approveProcessServices.RejectPrePayRequestForProjectManager(rejectRequest.UserId, rejectRequest.CurrentLoggingRole, rejectRequest.RequestId, message, rejectRequest.Feedback, feedBackStringBuilder);
            if (!canReject)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Reject action cannot be done " + message.ToString()
                });
            }
            // update request form status to reject
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(rejectRequest.RequestId);
            var requestFormDTO = new RequestFormDTO
            {
                Id = requestForm.Id,
                CreateAt = requestForm.CreateAt,
                Description = requestForm.Description,
                ExpectedMoney = requestForm.ExpectedMoney,
                CreatedBy = requestForm.CreatedBy,
                CampaignId = requestForm.CampaignId,
                TypeId = requestForm.TypeId,
                Status = Resource.RejectedStatus,
                FeedBack = feedBackStringBuilder.ToString()
            };
            var canUpdatedRequest = await requestFormServices.UpdateRequestFormAsync(requestFormDTO);
            if (canUpdatedRequest == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Cannot update Request Form"
                });
            }

            // update approve process status to reject
            var approveProcess = await approveProcessServices.GetApproveProcessesByRequestIdAndApproveIdAsync(rejectRequest.RequestId, rejectRequest.UserId);
            var updateApproveProcessDTO = new ApproveProcessRequest
            {
                Id = approveProcess.Id,
                ApproveNumber = approveProcess.ApproveNumber,
                ApproveStatus = Resource.RejectedStatus,
                RequestId = approveProcess.RequestId,
                ApproverId = approveProcess.ApproverId,
            };
            var canUpdatedApproveProcess = await approveProcessServices.UpdateApproveProcessAsync(updateApproveProcessDTO);
            return Ok(new
            {
                Success = true,
                Message = $"Update Request form with id = {requestForm.Id} to reject status"
            });
        }

        [HttpGet("approverequestforaccounting/{requestid}")]
        public async Task<IActionResult> ApproveRequestForAccounting(string userid, string currentLoggingRole, int requestid)
        {
            var message = new StringBuilder();
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(requestid);
            var accountingInCampaign = await requestFormServices.GetApproverForVolunteerLeader(requestForm.CampaignId);
            var projectManagerInCampaign = await requestFormServices.GetApproverForAccounting(requestForm.CampaignId);
            if (projectManagerInCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Project Manager not found"
                });
            }
            var canApprove = await approveProcessServices.ApproveRequestForAccounting(userid, currentLoggingRole, requestid, message);
            if (canApprove == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Approve action cannot be done " + message.ToString()
                });
            }
            // if create by accounting, create next new approve process for pm, auto approved
            if (requestForm.CreatedBy.Equals(projectManagerInCampaign.UserId))
            {
                var newApproveProvessForPM = new ApproveProcessRequest
                {
                    ApproveNumber = IntConstant.ThirdApproveNumber,
                    ApproveStatus = Resource.ApprovedStatus,
                    RequestId = requestid,
                    ApproverId = projectManagerInCampaign.UserId,
                };
                var newApproveProcessForAccounting = await approveProcessServices.CreateApproveProcessAsync(newApproveProvessForPM);
                // update all request form status to approved
                // calculate debt for request user
                var campaignMember = await campaignMemberService.GetCampaignMemberByUserIdAndCampaignIdAsync(requestForm.CreatedBy, requestForm.CampaignId);
                var updateRequestForm = new RequestFormDTO
                {
                    Id = requestForm.Id,
                    CreateAt = requestForm.CreateAt,
                    Description = requestForm.Description,
                    ExpectedMoney = requestForm.ExpectedMoney,
                    CreatedBy = requestForm.CreatedBy,
                    CampaignId = requestForm.CampaignId,
                    TypeId = requestForm.TypeId,
                    Status = Resource.ApprovedStatus,
                };
                var canUpdateRequestFormStatus = await requestFormServices.UpdateRequestFormAsync(updateRequestForm);
                var updateCampaignMember = (requestForm.TypeId == IntConstant.PrePayRequestType ?
                    new UpdateCampaignMemberDTO
                    {
                        Id = campaignMember.Id,
                        Debt = campaignMember.Debt + requestForm.ExpectedMoney,
                        IsActive = campaignMember.IsActive,
                    } :
                    new UpdateCampaignMemberDTO
                    {
                        Id = campaignMember.Id,
                        Debt = campaignMember.Debt - requestForm.ExpectedMoney,
                        IsActive = campaignMember.IsActive,
                    });
                var canUpdateCampaignMember = await campaignMemberService.UpdateCampaignMemberAsync(updateCampaignMember);
                return Ok(new
                {
                    Success = true,
                    Message = "Request is approved successfully",
                });
            }
            // create next new approve process for pm
            var newApproveProvess = new ApproveProcessRequest
            {
                ApproveNumber = IntConstant.ThirdApproveNumber,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = requestid,
                ApproverId = projectManagerInCampaign.UserId,
            };
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(newApproveProvess);

            return Ok(new
            {
                Success = true,
                Message = "Request is approved successfully",
            });
        }

        [HttpGet("approverequestforvolunteerleader/{requestid}")]
        public async Task<IActionResult> ApproveRequestForVolunteerLeader(string userid, string currentLoggingRole, int requestid)
        {
            var message = new StringBuilder();
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(requestid);
            var accountingInCampaign = await requestFormServices.GetApproverForVolunteerLeader(requestForm.CampaignId);
            var projectManagerInCampaign = await requestFormServices.GetApproverForAccounting(requestForm.CampaignId);
            if (accountingInCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Accounting not found"
                });
            }
            var canApprove = await approveProcessServices.ApproveRequestForLeader(userid, currentLoggingRole, requestid, message);
            if (canApprove == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Approve action cannot be done " + message.ToString()
                });
            }
            // if create by accounting, create next new approve process for pm, auto approved
            if (requestForm.CreatedBy.Equals(accountingInCampaign.UserId))
            {
                var newApproveProvessForAccounting = new ApproveProcessRequest
                {
                    ApproveNumber = IntConstant.SecondApproveNumber,
                    ApproveStatus = Resource.ApprovedStatus,
                    RequestId = requestid,
                    ApproverId = accountingInCampaign.UserId,
                };
                var newApproveProcessForAccounting = await approveProcessServices.CreateApproveProcessAsync(newApproveProvessForAccounting);
                // pushing for project manager
                var newApproveProvessForPM = new ApproveProcessRequest
                {
                    ApproveNumber = IntConstant.ThirdApproveNumber,
                    ApproveStatus = Resource.ProcessStatus,
                    RequestId = requestid,
                    ApproverId = projectManagerInCampaign.UserId,
                };
                var newApproveProcessForPM = await approveProcessServices.CreateApproveProcessAsync(newApproveProvessForPM);
                return Ok(new
                {
                    Success = true,
                    Message = "Request is approved successfully",
                });
            }
            // create next new approve process for accounting
            var newApproveProvess = new ApproveProcessRequest
            {
                ApproveNumber = IntConstant.SecondApproveNumber,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = requestid,
                ApproverId = accountingInCampaign.UserId,
            };
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(newApproveProvess);
            if (newApproveProcess == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Cannot create new Approve Process for accounting"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request is approved successfully",
            });
        }

        [HttpGet("approverequestforprojectmanager/{requestid}")]
        public async Task<IActionResult> ApproveRequestForProjectManager(string userid, string currentLoggingRole, int requestid)
        {
            var message = new StringBuilder();
            var canApprove = await approveProcessServices.ApproveRequestForProjectManager(userid, currentLoggingRole, requestid, message);
            if (canApprove == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Approve action cannot be done " + message.ToString()
                });
            }
            var requestForm = await _dbContext.RequestForms.FindAsync(requestid);
            // calculate debt for request user
            var campaignMember = await campaignMemberService.GetCampaignMemberByUserIdAndCampaignIdAsync(requestForm.CreatedBy, requestForm.CampaignId);
            if (campaignMember == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Campaign Member not found"
                });
            }
            // update status for request form
            requestForm.Status = Resource.ApprovedStatus;
            var canUpdateRequestFormStatus =  _dbContext.RequestForms.Update(requestForm);
            _dbContext.SaveChanges();
            var updateCampaignMember = requestForm.TypeId == IntConstant.PrePayRequestType ? new UpdateCampaignMemberDTO
            {
                Id = campaignMember.Id,
                Debt = campaignMember.Debt + requestForm.ExpectedMoney,
                IsActive = campaignMember.IsActive,
            } : new UpdateCampaignMemberDTO
                {
                    Id = campaignMember.Id,
                    Debt = campaignMember.Debt - requestForm.ExpectedMoney,
                    IsActive = campaignMember.IsActive,
                };
            var canUpdateCampaignMember = await campaignMemberService.UpdateCampaignMemberAsync(updateCampaignMember);
            if (!canUpdateCampaignMember)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Cannot update Campaign Member"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Request is approved successfully",
            });
        }
    }
}

using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.RequestFormServices;
using FALOFinancialProofing.Services.VoucherServices;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveProcessController : ControllerBase
    {
        private readonly IApproveProcessServices approveProcessServices;
        private readonly IRequestFormServices requestFormServices;
        private readonly IVoucherServices voucherServices;

        public ApproveProcessController(IApproveProcessServices approveProcessServices, IRequestFormServices requestFormServices, IVoucherServices voucherServices)
        {
            this.approveProcessServices = approveProcessServices;
            this.requestFormServices = requestFormServices;
            this.voucherServices = voucherServices;
        }

        [HttpGet("getallprepayrequestforvolunteerleader/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForVolunteerLeader(string userid, string currentLoggingRole)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForVolunteerLeader(userid, currentLoggingRole);
            return Ok(result);
        }

        [HttpGet("getallprepayrequestforaccounting/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForAccounting(string userid, string currentLoggingRole)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForAccounting(userid, currentLoggingRole);
            return Ok(result);
        }

        [HttpGet("approveprepayrequestforvolunteerleader/{requestid}")]
        public async Task<IActionResult> ApprovePrepayRequestForVolunteerLeader(string userid, string currentLoggingRole, int requestid)
        {
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(requestid);
            var accountingInCampaign = await requestFormServices.GetApproverForVolunteerLeader(requestForm.CampaignId);
            if (accountingInCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Accounting not found"
                });
            }
            var canApprove = await approveProcessServices.ApprovePrePayRequestForLeader(userid, currentLoggingRole, requestid);
            if (canApprove == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Approve action cannot be done"
                });
            }
            // create next new approve process for accounting
            var newApproveProvess = new ApproveProcessRequest
            {
                ApproveNumber = 2,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = requestid,
                ApproverId = accountingInCampaign.UserId,
            };
            var newApproveProcess = await approveProcessServices.CreateApproveProcessAsync(newApproveProvess);
            if(newApproveProcess == null)
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

        [HttpGet("rejectprepayrequestforvolunteerleader/{requestid}")]
        public async Task<IActionResult> RejectPrePayRequestForVolunteerLeader(string userid, string currentLoggingRole, int requestid)
        {
            var canReject = await approveProcessServices.RejectPrePayRequestForLeader(userid, currentLoggingRole, requestid);
            if (!canReject)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Reject action cannot be done"
                });
            }
            // update request form status to reject
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(requestid);
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
            var approveProcess = await approveProcessServices.GetApproveProcessesByRequestIdAndApproveIdAsync(requestid, userid);
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

        [HttpGet("approveprepayrequestforaccounting/{requestid}")]
        public async Task<IActionResult> ApprovePrepayRequestForAccounting(string userid, string currentLoggingRole, int requestid)
        {
            return Ok(new
            {
                Success = true,
                Message = "You have to submit pre-pay voucher first",
                Data = new
                {
                    Userid = userid,
                    CurrentLoggingRole = currentLoggingRole,
                    RequestId = requestid
                }
            });
        }


        [HttpPost("approveprepayrequestforaccounting")]
        public async Task<IActionResult> ApprovePrePayRequestAndSubmitVoucherForAccounting(string userid, string currentLoggingRole, int requestid, List<IFormFile> voucherFiles)
        {
            var requestForm = await requestFormServices.GetRequestFormByIdAsync(requestid);
            var projectmanagerInCampaign = await requestFormServices.GetApproverForAccounting(requestForm.CampaignId);
            if (projectmanagerInCampaign == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Project Manager not found"
                });
            }
            var canApprove = await approveProcessServices.ApprovePrePayRequestForAccounting(userid, currentLoggingRole, requestid);
            if (canApprove == false)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Approve action cannot be done"
                });
            }
            // create next new approve process for accounting
            var newApproveProvess = new ApproveProcessRequest
            {
                ApproveNumber = 3,
                ApproveStatus = Resource.ProcessStatus,
                RequestId = requestid,
                ApproverId = projectmanagerInCampaign.UserId,
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
            // save voucher file
            var vouchers = await requestFormServices.SaveUploadedVoucherAsync(newApproveProcess.Id, voucherFiles);
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
                Message = "Request is approved successfully",
            });
        }

    }
}

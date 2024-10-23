using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.RequestFormServices;
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

        public ApproveProcessController(IApproveProcessServices approveProcessServices, IRequestFormServices requestFormServices)
        {
            this.approveProcessServices = approveProcessServices;
            this.requestFormServices = requestFormServices;
        }

        [HttpGet("getallprepayrequestforvolunteerleader/{userid}")]
        public async Task<IActionResult> GetAllPrePayRequestForVolunteerLeader(string userid, string currentLoggingRole)
        {
            var result = await approveProcessServices.GetAllPrepayRequestForVolunteerLeader(userid, currentLoggingRole);
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

    }
}

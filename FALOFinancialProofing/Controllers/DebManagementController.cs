using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.DebManagementServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DebManagementController : ControllerBase
    {
        private readonly IDebManagementServices _debManagementService;

        public DebManagementController(IDebManagementServices debManagementService)
        {
            _debManagementService = debManagementService;
        }

        [HttpGet("getdebtlistforpmb")]   
        [RoleAttribute(AppRole.ProjectManagementBoard)]
        public async Task<IActionResult> GetDebManagementForPMB(string userId, string currentLoggingRole,
            int? campaignId,
            string? emailFilter,
            bool? isActive,
            int page = IntConstant.PageNumberDefault
            )
        {
            var message = new StringBuilder();
            var userWithDebList = await _debManagementService.GetDebManagementForPMB(userId, currentLoggingRole, message);
            if(userWithDebList.IsNullOrEmpty())
            {
                return Ok(new
                {
                    Success = false,
                    Message = message.ToString()
                });
            }

            var filteredResult = userWithDebList.AsEnumerable();
            if (campaignId != null)
            {
                filteredResult = filteredResult.Where(x => x.CampaignId == campaignId).ToList();
            }
            if (!string.IsNullOrEmpty(emailFilter))
            {
                filteredResult = filteredResult.Where(x => x.UserEmail.ToLower().Contains(emailFilter)).ToList();
            }
            if(isActive != null)
            {
                filteredResult = filteredResult.Where(x => x.IsActive == isActive).ToList();
            }
            var totalRecords = filteredResult.Count();
            var pagedResult = filteredResult
            .Skip((page - 1) * IntConstant.PageSizeForDeb)
                .Take(IntConstant.PageSizeForDeb)
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
                Message = "User Deb list retrieved successfully.",
                Data = response
            });


        }


        [HttpGet("getdebtlistforaccounting")]
        [RoleAttribute(AppRole.Accounting)]
        public async Task<IActionResult> GetDebManagementForAccounting(string userId, string currentLoggingRole,
            int campaignId,
            string? emailFilter,
            bool? isActive,
            int page = IntConstant.PageNumberDefault)
        {
            var message = new StringBuilder();
            var userWithDebList = await _debManagementService.GetDebManagementForAccounting(userId, currentLoggingRole, message, campaignId);
            var filteredResult = userWithDebList.AsEnumerable();
            if (!string.IsNullOrEmpty(emailFilter))
            {
                filteredResult = filteredResult.Where(x => x.UserEmail.ToLower().Contains(emailFilter)).ToList();
            }
            if (isActive != null)
            {
                filteredResult = filteredResult.Where(x => x.IsActive == isActive).ToList();
            }
            var totalRecords = filteredResult.Count();
            var pagedResult = filteredResult
            .Skip((page - 1) * IntConstant.PageSizeForDeb)
                .Take(IntConstant.PageSizeForDeb)
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
                Message = "User Deb list retrieved successfully.",
                Data = response
            });
        }


    }
}

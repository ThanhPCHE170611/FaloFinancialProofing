using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Services.AccountingBookServices;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountingBookController : ControllerBase
    {
        private readonly IAcccountingBookServices accountingBookServices;

        public AccountingBookController(IAcccountingBookServices accountingBookServices)
        {
            this.accountingBookServices = accountingBookServices;
        }

        [HttpPost("uploadaccountingbook")]
        [RoleAttribute(AppRole.Accounting)]
        public async Task<IActionResult> UploadAccountingBook([FromForm]CreateAccountingBookRequest request)
        {
            var errorMessages = new StringBuilder();
            var newAccountingBook = await accountingBookServices.CreateAccountingRequest(request, errorMessages);
            if (newAccountingBook == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = errorMessages.ToString()
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Accounting book uploaded successfully",
                Data = newAccountingBook,
            });
        }

        [AllowAnonymous]
        [HttpGet("getaccountingbookincampaign")]
        public async Task<IActionResult> GetAccountingBookInCampaign(int campaignId)
        {
            var accountingBook = await accountingBookServices.GetAccountingBookInCampaign(campaignId);
            if (accountingBook == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Accounting book not found"
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Accounting book found",
                Data = accountingBook,
            });
        }

        [HttpGet("getallaccountingbookinproject")]
        public async Task<IActionResult> GetAllAccountingBookInProject(int projectId, string? name,
            int page = IntConstant.PageNumberDefault)
        {
            var accountingBooks = await accountingBookServices.GetAllAccountingBookInProject(projectId);
            if (!accountingBooks.Any())
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Accounting books not found"
                });
            }

            var filteredAttachmentFiles = accountingBooks.AsQueryable();

            if (String.IsNullOrEmpty(name))
            {
                filteredAttachmentFiles = filteredAttachmentFiles
                    .Where(x => x.FilePath.ToLower().Contains(name.ToLower()));
            }
            var totalRecords = filteredAttachmentFiles.Count();
            var pagedResult = filteredAttachmentFiles
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
                Message = "Accounting books found",
                Data = response
            });
        }

        [AllowAnonymous]
        [HttpGet("downloadaccountingbook/{fileName}")]
        public async Task<IActionResult> DownloadAccountingBookByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await accountingBookServices.DownloadAccountingBookFileByFileName(fileName);
            if (fileBytes == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "File not found",
                });
            }

            return File(fileBytes, contentType, downloadFileName);
        }

        [HttpDelete("deleteaccountingbook/{fileName}")]
        [RoleAttribute(AppRole.Accounting)]
        public async Task<IActionResult> DeleteAccountingBookInCampaign(string fileName)
        {
            var message = new StringBuilder();
            var canDeleteAccountingBook = await accountingBookServices.DeleteAccountingBook(fileName, message);
            if (!canDeleteAccountingBook)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Delete Accounting Book failed: " + message
                });
            }
            return Ok(new
            {
                Success = true,
                Message = "Accounting book deleted successfully"
            });
        }
    }
}

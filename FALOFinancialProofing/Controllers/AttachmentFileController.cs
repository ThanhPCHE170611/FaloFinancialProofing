using FALOFinancialProofing.Constant;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentFileController : ControllerBase
    {
        private readonly IAttachmentFileServices attachmentFileServices;

        public AttachmentFileController(IAttachmentFileServices attachmentFileServices)
        {
            this.attachmentFileServices = attachmentFileServices;
        }

        [HttpGet("downloadprepayattachmentfile/{fileName}")]
        public async Task<IActionResult> DownloadPrePayAttachmentFileByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await attachmentFileServices.DownloadPrePayAttachmentFileByFileName(fileName);

            if (fileBytes == null)
            {
                return Ok(new { Success = false,
                    Message = "File not found",
                    });
            }

            return File(fileBytes, contentType, downloadFileName);
        }

        [HttpGet("downloadpaymentattachmentfile/{fileName}")]
        public async Task<IActionResult> DownloadPaymentAttachmentFileByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await attachmentFileServices.DownloadPaymentAttachmentFileByFileName(fileName);

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

        [HttpGet("getallcurrentattachmentincampaign/{campaignId}")]
        public async Task<IActionResult> GetAllCurrentAttachmentInCampaign(int campaignId, string? type,
            int page = IntConstant.PageNumberDefault)
        {
            var attachmentFiles = await attachmentFileServices.GetAllCurrentAttachmentInCampaign(campaignId);
            if (attachmentFiles == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Attachment files not found"
                });
            }
            var filteredAttachmentFiles = attachmentFiles.AsEnumerable();
            if (type != null)
            {
                var typeInt = (type.Equals("Pre-Pay") ? 1 : 2);
                filteredAttachmentFiles = filteredAttachmentFiles.Where(x => x.RequestForm.TypeId == typeInt).ToList();
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
                Message = "Attachment retrieved successfully.",
                Data = response
            });
        }

        [HttpGet("gettotalnumbercurrentattachmentincampaign/{campaignId}")]
        public async Task<IActionResult> GetTotalNumberCurrentAttachmentInCampaign(int campaignId)
        {
            var attachmentFiles = await attachmentFileServices.GetAllCurrentAttachmentInCampaign(campaignId);
            if (attachmentFiles == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Attachment files not found"
                });
            }
            var totalRecords = attachmentFiles.Count();
            return Ok(new
            {
                Success = true,
                Message = "Attachment retrieved successfully.",
                Data = totalRecords
            });
        }

        [HttpGet("getnumberallattachmentfileincampain/{campaignId}")]
        public async Task<IActionResult> GetlNumberAllAttachmentFileInCampain(int campaignId)
        {
            var requests = await attachmentFileServices.GetAllAttachmentInCampaignByRequest(campaignId);
            if (requests == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Attachment files not found"
                });
            }
            var totalRecords = requests.Count();
            return Ok(new
            {
                Success = true,
                Message = "Attachment retrieved successfully.",
                Data = totalRecords
            });
        }
    }
}

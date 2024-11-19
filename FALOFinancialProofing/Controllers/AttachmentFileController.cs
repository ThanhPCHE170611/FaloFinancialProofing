using FALOFinancialProofing.Constant;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpGet("downloadattachmentfilewithnotypebyfilename/{fileName}")]
        public async Task<IActionResult> DownloadAttachmentFileWithNoTypeByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await attachmentFileServices.DownloadAttachmentFileWithNoTypeByFileName(fileName);

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
        public async Task<IActionResult> GetAllCurrentAttachmentInCampaign(int campaignId, 
            string? type,
            string? name,
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
                var typeInt = 0;
                if(type == "prepay")
                {
                    typeInt = IntConstant.PrePayRequestType;
                }
                else if(type == "payment")
                {
                    typeInt = IntConstant.PaymentRequestType;
                }
                filteredAttachmentFiles = filteredAttachmentFiles.Where(x => x.RequestForm.TypeId == typeInt).ToList();
            }
            if (!String.IsNullOrEmpty(name))
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

        [HttpGet("getallpaymentattachmentincampaignwithrequest/{campaignId}")]
        public async Task<IActionResult> GetAllPaymentAttachmentInCampaignWithRequest(int campaignId,
            string? name,
            int page = IntConstant.PageNumberDefault)
        {
            var attachmentFiles = await attachmentFileServices.GetAllPaymentAttachmentInCampaignWithRequest(campaignId);
            if(!attachmentFiles.Any())
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Attachment files not found"
                });
            }

            var filteredAttachmentFiles = attachmentFiles.AsEnumerable();
            if (!String.IsNullOrEmpty(name))
            {
                filteredAttachmentFiles = filteredAttachmentFiles
                    .Where(x => x.AttachmentFilePath != null && x.AttachmentFilePath.ToLower().Contains(name.ToLower()));
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
    }
}

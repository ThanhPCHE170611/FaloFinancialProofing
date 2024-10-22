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

            // Trả về file để trình duyệt tự động tải xuống
            return File(fileBytes, contentType, downloadFileName);
        }
    }
}

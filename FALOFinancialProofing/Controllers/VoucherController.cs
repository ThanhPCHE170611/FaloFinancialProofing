using FALOFinancialProofing.Services.AttachmentFIleServices;
using FALOFinancialProofing.Services.VoucherServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherServices voucherServices;

        public VoucherController(IVoucherServices voucherServices)
        {
            this.voucherServices = voucherServices;
        }

        [HttpGet("downloadprepayvoucherfile/{fileName}")]
        public async Task<IActionResult> DownloadVoucherFileByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await voucherServices.DownloadPrePayAttachmentFileByFileName(fileName);

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

        [HttpGet("downloadpaymentvoucherfile/{fileName}")]
        public async Task<IActionResult> DownloadPaymentVoucherFileByFileName(string fileName)
        {
            var (fileBytes, contentType, downloadFileName) = await voucherServices.DownloadPaymentAttachmentFileByFileName(fileName);

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
    }
}

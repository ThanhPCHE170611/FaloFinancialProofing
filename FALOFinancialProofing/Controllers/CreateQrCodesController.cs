using FALOFinancialProofing.DTOs.CreateQrCodeDTO;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.BankServices;
using FALOFinancialProofing.Services.CreateQrCodeServices;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateQrCodesController : ControllerBase
    {
        private readonly ICreateQrCodeService _createQrCodeService;
        private readonly BankService1 bankService1;
        private readonly IBankService bankService;

        public CreateQrCodesController(ICreateQrCodeService createQrCodeService, BankService1 bankService1, IBankService bankService)
        {
            _createQrCodeService = createQrCodeService;
            this.bankService1 = bankService1;
            this.bankService = bankService;
        }
        [HttpPost("CreateQrCode")]
        public async Task<IActionResult> CreateQrCode([FromBody] CreateQrCodeRequest createQrCodeRequest)
        {
            StringBuilder message = new StringBuilder();
            var checkValid = await _createQrCodeService.ValidateQrCodeCreate(createQrCodeRequest, message);
            if (!checkValid)
            {
                return Ok(new
                {
                    Success = checkValid,
                    Message = message.ToString()
                });
            }
            CreateQrCode createQrCode = new CreateQrCode
            {
                UserId = createQrCodeRequest.UserId,
                Amount = createQrCodeRequest.Amount,
                IsPaid = false
            };
            checkValid = await _createQrCodeService.CreateQrCodeAsync(createQrCode);
            message.Append("Create QrCode successfully. ");

            // tìm bank bằng bankId // muốn tối ưu phải đặt trước createQrCode
            var bankInformation = await bankService.GetBankByIdAsync(createQrCodeRequest.BankId);
            if (bankInformation == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Bank not found"
                });
            }

            BankRequest bankRequest = new BankRequest()
            {
                accountNo = bankInformation.AccountNumber,
                accountName = bankInformation.OwnerName,
                acqId = bankInformation.acqId,
                amount = createQrCodeRequest.Amount,
                addInfo = $"C{createQrCodeRequest.CampaignId}CC{createQrCode.Id}Q ",
                format = "text",
                template = "print"
            };
            var response = await bankService1.GetQRCode(bankRequest);
            if (response == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Error In Genenerate Qr Code!"
                });
            }
            // lấy ra qrDataURL trong response.data
            var image = bankService1.ConvertBase64ToImage(response.data.qrDataURL);

            return File(image, "image/png");
        }
        // Get QrCode by id de lay lsgd

        [HttpGet("GetCreateQrById/{id}")]
        public async Task<IActionResult> GetCreateQrById(int id)
        {
            var createQrCode = await _createQrCodeService.GetQrCodeByIdAsync(id);
            if (createQrCode == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"CreateQrCode with Id = {id} not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "GetCreateQrById retrieved successfully.",
                Data = createQrCode
            });
        }
    }
}

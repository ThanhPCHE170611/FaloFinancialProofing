using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CreateQrCodeServices;
using FALOFinancialProofing.Services.TransactionLogsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {
        private readonly WebHookService _webHookService;
        private readonly string secure_token = "abcdef";
        private readonly ITransactionLogService _transactionLogService;
        private readonly ICampaignService _campaignService;
        private readonly ICreateQrCodeService _createQrCodeService;
        public WebHooksController(WebHookService webHookService, ITransactionLogService transactionLogService, ICampaignService campaignService, ICreateQrCodeService createQrCodeService)
        {
            _webHookService = webHookService;
            _transactionLogService = transactionLogService;
            _campaignService = campaignService;
            _createQrCodeService = createQrCodeService;
        }
        // mẫu TransactionRequest
        [HttpPost("Create-Webhook")]
        public async Task<IActionResult> CreateWebhook([FromBody] WebHookDataRequest item)
        {
            var data = await _webHookService.CreateWebHookAsync(item);
            return Ok(data);
        }

        [HttpGet("GetWebHookDetailById/{id}")]
        public async Task<IActionResult> GetWebHookDetailByIdAsync(int id)
        {

            var data = await _webHookService.GetWebHookDetailByIdAsync(id);
            return Ok(data);
        }

        [HttpPut("Update-Webhook/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] WebHookDataRequest webHookDataRequest)
        {
            var data = await _webHookService.UpdateWebHookAsync(id, webHookDataRequest);
            return Ok(data);
        }

        [HttpDelete("Delete-Webhook/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var data = await _webHookService.DeleteWebHookByIdAsync(id);
            return Ok(data);
        }
        [HttpDelete("Delete-Webhooks/{url}")]
        public async Task<IActionResult> DeleteItem(string url)
        {
            var data = await _webHookService.DeleteAllWebHooksByUrlAsync(url);
            return Ok(data);
        }

        // xử lí giao dịch vào hoặc ra hoặc cả 2
        [HttpPost("Transaction-Transfer")]
        public async Task<IActionResult> TransactionTransfer([FromBody] TransactionRequest item)
        {
            //if (!HttpContext.Request.Headers.TryGetValue("secure-token", out var secureToken))
            //{

            //    if (!secureToken.ToString().Equals("secure_token"))
            //    {
            //        return Unauthorized();
            //    }
            //}

            // trích nội dung giao dịch
            // thực hiện kiểm tra có orderId đấy không
            // thực hiện kiểm tra có campaignId đấy không
            // nếu có thực hiện lưu vào transactionlog các id: createQr, Campaignid, orderId, amount, description, trạng thái qr
            // phải xử lí nó là giao dịch gì
            // kiểm tra giao dịch tồn tại trong transactionLog chưa
            try
            {
                string format = "yyyy-MM-dd HH:mm:ss";
                foreach (var transaction in item.data)
                {
                    var transactionLogByCassoTransactionId = await _transactionLogService.GetTransactionLogByCassoTransactionIdAsync(transaction.id);
                    if (transactionLogByCassoTransactionId != null)
                    {
                        continue;
                    }
                    if (transaction.description == null)
                    {
                        continue;
                    }

                    var stringSplit = transaction.description.Trim().Split('.');
                    string description = null;
                    if (stringSplit.Length >= 3)
                    {
                        description = stringSplit[3].Trim();
                    }
                    if (!description.Contains("CC") && !description.StartsWith("C"))
                    {
                        continue;
                    }
                    string[] descriptionSplit = description.Split('C', 'Q');
                    if (int.TryParse(descriptionSplit[1], out int CampaignId) && int.TryParse(descriptionSplit[3], out int CreateQrId))
                    {
                        var campaign = await _campaignService.GetCampaignByCampaignIdAsync(CampaignId);
                        var createQrCode = await _createQrCodeService.GetQrCodeByIdAsync(CreateQrId);
                        if (campaign == null || createQrCode == null)
                        {
                            continue;
                        }

                        // lưu vào transactionlog
                        TransactionLog transactionLog = new TransactionLog()
                        {
                            CreateQrCodeId = createQrCode.Id,
                            Amount = transaction.amount,
                            CampaignId = campaign.CampaignId,
                            Description = description,
                            TransactionDate = DateTime.ParseExact(transaction.when, format, CultureInfo.InvariantCulture),
                            CassoTransactionId = transaction.id,
                            tid = transaction.tid
                        };
                        await _transactionLogService.CreateTransactionLogAsync(transactionLog);
                        // cap nhat trang thai cua qr
                        createQrCode.IsPaid = true;
                        await _createQrCodeService.UpdateCreateQrCodeAsync(createQrCode);
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"TransactionTransfer: {ex.Message}");
            }

            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Transaction success",
            });
        }

        // call when user confirm payment
        [HttpPost("SyncTransaction/{accountNumber}")]
        public async Task<IActionResult> SyncTransaction(string accountNumber)
        {
            SyncResponse syncResponse = null!;
            StringBuilder message = new StringBuilder();
            try
            {
                if (accountNumber == null)
                {
                    message.Append("Account number is required");
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = message.ToString(),
                    });
                }
                SyncRequest syncRequest = new SyncRequest()
                {
                    bank_acc_id = accountNumber,
                };
                syncResponse = await _webHookService.TransactionSync(syncRequest);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"SyncTransaction: {ex.Message}");
            }
            return Ok(syncResponse);
        }
    }
}

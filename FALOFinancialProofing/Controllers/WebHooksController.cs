using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Transactions;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {
        //private const string APIKey =";
        private const string SecureToken = "EE965E354266AE15B5BFC92DC3416";
        private readonly WebHookService _webHookService;
        public WebHooksController(WebHookService webHookService)
        {
            _webHookService = webHookService;
        }
        [HttpPost("create-webhook")]
        public IActionResult CreateItem([FromBody] TransactionRequest item)
        {
            // Xử lý logic tạo item
            var httpContext = HttpContext.Request.Body;
            return Ok(new { message = "Item created", item });
        }

        //[HttpGet("read-webhook/{id}")]
        //public IActionResult ReadItem(int id)
        //{
        //    // Xử lý logic đọc item
        //    var item = new Item { Id = id, Name = "Sample Item", Description = "This is a sample item." };
        //    return Ok(item);
        //}

        //[HttpPut("update-webhook/{id}")]
        //public IActionResult UpdateItem(int id, [FromBody] Item updatedItem)
        //{
        //    // Xử lý logic cập nhật item
        //    updatedItem.Id = id;
        //    return Ok(new { message = "Item updated", updatedItem });
        //}

        //[HttpDelete("delete-webhook/{id}")]
        //public IActionResult DeleteItem(int id)
        //{
        //    // Xử lý logic xóa item
        //    return Ok(new { message = "Item deleted", id });
        //}

        [HttpPost("transaction-transfer")]
        public async Task<IActionResult> TransactionTransfer([FromBody] TransactionRequest item)
        {



            // Xử lý logic tạo item
            var httpContext = HttpContext.Request.Body;
            return Ok(new { message = "Item created", item });
        }

        // call when user confirm payment
        [HttpPost("SyncTransaction/{accountNumber}")]
        public async Task<IActionResult> SyncTransaction(string accountNumber)
        {
            bool checkSuccess = false;
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
                checkSuccess = await _webHookService.TransactionSync(accountNumber);
                if (checkSuccess)
                {
                    message.Append("Sync transaction success");
                }
                else
                {
                    message.Append("Sync transaction failed");
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"SyncTransaction: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = checkSuccess,
                Message = message.ToString(),
            });
        }
    }
}

using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {
        private const string APIKey = "AK_CS.a86bc6e09b7111ef8a02890bf6befcfe.7ck5OvlLrnfpprkrZOWgPqT8PAvkUVHvfeZ3A7E2sjvqjVYbtUnsTUULw2ISqa0QKLY4goIL";
        private const string SecureToken = "EE965E354266AE15B5BFC92DC3416";
        [HttpPost("create-webhook")]
        public IActionResult CreateItem([FromBody] TransactionRequest item)
        {
            // Xử lý logic tạo item
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.TransactionLogsServices;
using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using Microsoft.VisualBasic;
using FALOFinancialProofing.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FALOFinancialProofing.Constant;
using FALOFinancialProofing.DTOs.CreateCampaignRequestDTO;
using FALOFinancialProofing.Utilities;
using System.Text;
using FALOFinancialProofing.DTOs.UserDTOs;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionLogsController : ControllerBase
    {
        private readonly ITransactionLogService _transactionLogService;

        public TransactionLogsController(ITransactionLogService transactionLogService)
        {
            _transactionLogService = transactionLogService;
        }

        // GET: api/TransactionLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionLog>>> GetTransactionLogs()
        {
            return Ok(await _transactionLogService.GetAllTransactionLogsAsync());
        }

        // GET: api/TransactionLogs/5
        [HttpGet("GetTransactionLog/{id}")]
        public async Task<ActionResult<TransactionLog>> GetTransactionLog(int id)
        {

            var transactionLog = await _transactionLogService.GetTransactionLogByIdAsync(id);

            if (transactionLog == null)
            {
                return NotFound();
            }

            return transactionLog;
        }
        // lịch sử chuyển tiền cua nguoi dung
        [HttpGet("GetUserTransactionLogs/{userId}")]
        public async Task<ActionResult> GetUserTransactionLogs(string? searchInput, string userId, int currentPage = IntConstant.PageNumberDefault)
        {
            List<UserTransactionHistory> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                data = (await _transactionLogService.GetUserTransactionsByUserIdAsync(userId)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get User Transaction Logs Successfully Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.CampaignName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<UserTransactionHistory>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Get User Transaction Logs: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get User Transaction Logs Successfully!",
                Data = filterPagingData
            });
        }

        //[HttpGet("GetCampaignTransactionLogs/{campaignId}")]
        //public async Task<ActionResult> GetCampaignTransactionLogs(string? searchInput, int campaignId, int currentPage = IntConstant.PageNumberDefault)
        //{

        //    var transactionLog = await _transactionLogService.GetUserTransactionsByCampaignIdAsync(campaignId);

        //    if (transactionLog == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!string.IsNullOrEmpty(searchInput))
        //    {
        //        searchInput = searchInput.Trim();
        //        transactionLog = transactionLog.FindAll(x => ($"{x.CampaignName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase) || ($"{x.TransactionDate.ToShortDateString()}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
        //    }
        //    return Ok(new ApiResponse()
        //    {
        //        Data = transactionLog,
        //        Success = true,
        //        Message = "Get Campaign Transaction Logs Successfully!"
        //    });
        //}

        [HttpGet("GetCampaignTransactionLogs/{campaignId}")]
        public async Task<ActionResult> GetCampaignTransactionLogs(string? searchInput, int campaignId, int currentPage = IntConstant.PageNumberDefault)
        {
            List<UserTransactionHistory> data = null;
            FilterPagingData filterPagingData = new FilterPagingData();
            filterPagingData.CurrentPage = currentPage;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                data = (await _transactionLogService.GetUserTransactionsByCampaignIdAsync(campaignId)).ToList();
                if (data == null || data.Count == 0)
                {
                    return Ok(new ApiResponse()
                    {
                        Success = false,
                        Message = "Get All TransactionLogs By CampaignId Failed!",
                        Data = filterPagingData
                    });
                }
                if (!string.IsNullOrEmpty(searchInput))
                {
                    searchInput = searchInput.Trim();
                    data = data.FindAll(x => ($"{x.CampaignName}").Contains(searchInput, StringComparison.OrdinalIgnoreCase));
                }
                filterPagingData.DataCount = data.Count;
                data = PaginationHelper.Paginate<UserTransactionHistory>(data.AsQueryable(), currentPage, IntConstant.PageSize).ToList();
                filterPagingData.Data = data;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Get All TransactionLogs By CampaignId: {ex.Message}");
            }
            return Ok(new ApiResponse()
            {
                Success = true,
                Message = "Get All TransactionLogs By CampaignId Successfully!",
                Data = filterPagingData
            });
        }
        // PUT: api/TransactionLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateTransactionLog")]
        public async Task<IActionResult> PutTransactionLog([FromBody] UpdateTransactionLog UpdateTransactionLog)
        {
            var statusMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _transactionLogService.UpdateTransactionLogAsync(UpdateTransactionLog) != false ? "Update TransactionLog Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "Update TransactionLog Failed!";
                await Console.Out.WriteLineAsync("PutTransactionLog: Error");
            }

            return Content(statusMessage);
        }

        // POST: api/TransactionLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateTransactionLog", Name = "CreateTransactionLog")]
        public async Task<ActionResult<TransactionLog>> PostTransactionLog([FromBody] CreateTransactionLog createTransactionLog)
        {
            var statusMessage = "";
            try
            {
                //var url = Url.RouteUrl("CreateTransactionLog");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                statusMessage = await _transactionLogService.CreateTransactionLogAsync(createTransactionLog)
                    != false ? "Create TransactionLog Successfully!" : throw new Exception();

            }
            catch (Exception ex)
            {
                statusMessage = "Create TransactionLog Failed!";
                await Console.Out.WriteLineAsync($"PostTransactionLog: {ex.Message}");
            }

            return Content(statusMessage);
        }

        // DELETE: api/TransactionLogs/5
        [HttpDelete("DeleteTransactionLog/{id}")]
        public async Task<IActionResult> DeleteTransactionLog(int id)
        {
            var statusMessage = "";
            try
            {
                statusMessage = await _transactionLogService.DeleteTransactionLogAsync(id)
                    != false ? "DELETE TransactionLog Successfully!" : throw new Exception();
            }
            catch (Exception ex)
            {
                statusMessage = "DELETE TransactionLog Failed!";
                await Console.Out.WriteLineAsync("PostTransactionLog: Error");
            }

            return Content(statusMessage);
        }


    }
}

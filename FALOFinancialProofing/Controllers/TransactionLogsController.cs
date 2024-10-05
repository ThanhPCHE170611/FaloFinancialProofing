﻿using System;
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

        // PUT: api/TransactionLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateTransactionLog")]
        public async Task<IActionResult> PutTransactionLog([FromBody] UpdateTransactionLog UpdateTransactionLog)
        {
            var statusMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _transactionLogService.UpdateTransactionLogAsync(UpdateTransactionLog);
                statusMessage = "Update TransactionLog Successfully!";
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
        [HttpPost(Name = "CreateTransactionLog")]
        public async Task<ActionResult<TransactionLog>> PostTransactionLog([FromBody] CreateTransactionLog createTransactionLog)
        {
            var statusMessage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _transactionLogService.CreateTransactionLogAsync(createTransactionLog);
                statusMessage = "Create TransactionLog Successfully!";
            }
            catch (Exception ex)
            {
                statusMessage = "Create TransactionLog Failed!";
                await Console.Out.WriteLineAsync("PostTransactionLog: Error");
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
                await _transactionLogService.DeleteTransactionLogAsync(id);
                statusMessage = "Create TransactionLog Successfully!";
            }
            catch (Exception ex)
            {
                statusMessage = "Create TransactionLog Failed!";
                await Console.Out.WriteLineAsync("PostTransactionLog: Error");
            }

            return Content(statusMessage);
        }


    }
}
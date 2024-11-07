using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.BankAccountServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly FALOFinancialProofingDbContext _dbContext;

        public BankAccountController(IBankAccountService bankAccountService, FALOFinancialProofingDbContext dbContext)
        {
            _bankAccountService = bankAccountService;
            _dbContext = dbContext;
        }

        // GET: api/BankAccount
        [HttpGet]
        public async Task<ActionResult<List<BankAccount>>> GetAccounts()
        {
            // Gọi phương thức GetAccounts từ BankAccountService
            var accounts = await _bankAccountService.GetAccounts();
            return Ok(accounts);
        }

        // GET: api/BankAccount/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetAccount(int id)
        {
            var account = await _dbContext.BankAccounts.FindAsync(id);
            if (account == null)
            {
                return NotFound($"Bank account with ID {id} not found.");
            }
            return Ok(account);
        }

        // POST: api/BankAccount
        [HttpPost]
        public async Task<ActionResult<BankAccount>> CreateAccount(BankAccount account)
        {
            // Thêm tài khoản mới vào cơ sở dữ liệu
            _dbContext.BankAccounts.Add(account);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        // PUT: api/BankAccount/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, BankAccount updatedAccount)
        {
            if (id != updatedAccount.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            var existingAccount = await _dbContext.BankAccounts.FindAsync(id);
            if (existingAccount == null)
            {
                return NotFound($"Bank account with ID {id} not found.");
            }

            // Cập nhật các thông tin của tài khoản
            existingAccount.AccountNumber = updatedAccount.AccountNumber;
            existingAccount.AccountName = updatedAccount.AccountName;
            existingAccount.BankCode = updatedAccount.BankCode;
            existingAccount.Balance = updatedAccount.Balance;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/BankAccount/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _dbContext.BankAccounts.FindAsync(id);
            if (account == null)
            {
                return NotFound($"Bank account with ID {id} not found.");
            }

            _dbContext.BankAccounts.Remove(account);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

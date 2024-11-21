using FALOFinancialProofing.Models;
using FALOFinancialProofing.Services.BankServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;
        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }

        //[HttpGet("GetAllBanks")]
        //public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
        //{
        //    return Ok(await _bankService.GetAllBanksAsync());
        //}

        // bank not in used
        [HttpGet("GetAllBanks")]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
        {
            return Ok(await _bankService.GetAllBanksAsync());
        }

        [HttpGet("GetBankById/{BankId}")]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBankById(int BankId)
        {
            return Ok(await _bankService.GetBankByIdAsync(BankId));
        }
    }
}

using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.BankAccountServices
{
    public interface IBankAccountService
    {
        Task<List<BankAccount>> GetAccounts();
    }
}

using FALOFinancialProofing.DTOs.BankDTO;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.BankServices
{
    public interface IBankService
    {
        Task<bool> CreateBankAsync(Bank Bank);
        Task<BankInformation> GetBankByIdAsync(int id);
        Task<IEnumerable<Bank>> GetAllBanksAsync();
        Task<bool> UpdateBankAsync(Bank Bank);
        Task<bool> DeleteBankAsync(int id);
        //Task<bool> UpdateBankToSystemAsync(Bank Bank);
        Task<BankInformation> GetBankByCampaignIdAsync(int CampaignId);
        Task<bool> SyncBankAccountsToBankDb(List<CassoBankAccount> cassoBankAccounts);
    }
}

using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.TransactionLogsServices
{
    public interface ITransactionLogService
    {
        Task<bool> CreateTransactionLogAsync(CreateTransactionLog createTransactionLog);
        Task<bool> CreateTransactionLogAsync(TransactionLog createTransactionLog);
        Task<TransactionLog> GetTransactionLogByIdAsync(int id);
        Task<IEnumerable<TransactionLog>> GetAllTransactionLogsAsync();
        Task<bool> UpdateTransactionLogAsync(UpdateTransactionLog updateTransactionLog);
        Task<bool> DeleteTransactionLogAsync(int id);
        Task<TransactionLog> GetTransactionLogByCassoTransactionIdAsync(int id);
        Task<List<UserTransactionHistory>> GetUserTransactionsByUserIdAsync(string userId);
        Task<List<UserTransactionHistory>> GetUserTransactionsByCampaignIdAsync(int campaignId);
        Task<List<UserTransactionHistory>> GetMoneyOutTransactionsByCampaignIdAsync(int campaignId);
        Task<decimal> GetTotalMoneyOutByCampaignId(int campaignId);
    }
}

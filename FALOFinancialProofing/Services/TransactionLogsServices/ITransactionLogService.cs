using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.Models;

namespace FALOFinancialProofing.Services.TransactionLogsServices
{
    public interface ITransactionLogService
    {
        Task<bool> CreateTransactionLogAsync(CreateTransactionLog createTransactionLog);
        Task<TransactionLog> GetTransactionLogByIdAsync(int id);
        Task<IEnumerable<TransactionLog>> GetAllTransactionLogsAsync();
        Task<bool> UpdateTransactionLogAsync(UpdateTransactionLog updateTransactionLog);
        Task<bool> DeleteTransactionLogAsync(int id);
    }
}

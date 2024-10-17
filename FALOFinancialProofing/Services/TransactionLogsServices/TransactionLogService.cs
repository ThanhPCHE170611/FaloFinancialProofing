using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.TransactionLogsServices
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly IRepository<TransactionLog, int> _transactionLogRepository;

        public TransactionLogService(IRepository<TransactionLog, int> transactionLogRepository)
        {
            _transactionLogRepository = transactionLogRepository;
        }

        private TransactionLog ConvertToBaseEntity(CreateTransactionLog createTransactionLog)
        {
            var transactionLog = new TransactionLog
            {
                SenderID = createTransactionLog.SenderID,
                CampaignId = createTransactionLog.CampaignId,
                TransactionDate = createTransactionLog.TransactionDate,
                Amount = createTransactionLog.Amount,
                Description = createTransactionLog.Description,
                BankId = createTransactionLog.BankId
            };
            return transactionLog;
        }
        public async Task<bool> CreateTransactionLogAsync(CreateTransactionLog createTransactionLog)
        {
            try
            {
                if (createTransactionLog == null)
                {
                    throw new Exception("CreateTransactionLog is null");
                }
                var transactionLog = ConvertToBaseEntity(createTransactionLog);
                await _transactionLogRepository.InsertAsync(transactionLog);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateTransactionLog: {ex.Message}!");
            }

            return false;
        }

        private void ConvertToBaseEntity(TransactionLog SourceTransactionLog, UpdateTransactionLog DesTransactionLog)
        {
            SourceTransactionLog.SenderID = DesTransactionLog.SenderID;
            SourceTransactionLog.Amount = DesTransactionLog.Amount;
            SourceTransactionLog.BankId = DesTransactionLog.BankId;
            SourceTransactionLog.Description = DesTransactionLog.Description;
            SourceTransactionLog.CampaignId = DesTransactionLog.CampaignId;
        }

        public async Task<TransactionLog> GetTransactionLogByIdAsync(int id)
        {
            TransactionLog transactionLog = null!;
            try
            {
                transactionLog = await _transactionLogRepository.Get(id);
                if (transactionLog == null)
                {
                    throw new Exception("TransactionLog not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetTransactionLogById: {ex.Message}");
            }

            return transactionLog;
        }

        public async Task<IEnumerable<TransactionLog>> GetAllTransactionLogsAsync()
        {
            List<TransactionLog> data = null!;
            try
            {
                data = await _transactionLogRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllTransactionLogs: {ex.Message}");
            }

            return data;
        }
        // admin can update transaction logs
        public async Task<bool> UpdateTransactionLogAsync(UpdateTransactionLog updateTransactionLog)
        {
            TransactionLog transactionLog = null!;
            bool result = false;
            try
            {
                transactionLog = await _transactionLogRepository.Get(updateTransactionLog.Id);
                if (transactionLog == null)
                {
                    throw new Exception("TransactionLog not found!");
                }
                ConvertToBaseEntity(transactionLog, updateTransactionLog);
                result = await _transactionLogRepository.UpdateAsync(transactionLog);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateTransactionLog: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteTransactionLogAsync(int id)
        {
            TransactionLog transactionLog = null!;
            bool result = false;
            try
            {
                transactionLog = await _transactionLogRepository.Get(id);
                if (transactionLog == null)
                {
                    throw new Exception("TransactionLog not found!");
                }
                result = await _transactionLogRepository.DeleteAsync(transactionLog);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteTransactionLog: {ex.Message}");
            }

            return result;
        }
    }
}

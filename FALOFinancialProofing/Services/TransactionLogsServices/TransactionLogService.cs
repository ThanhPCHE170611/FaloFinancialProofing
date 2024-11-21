using FALOFinancialProofing.DTOs.TransactionLogsDTOs;
using FALOFinancialProofing.DTOs.UserDTOs;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.TransactionLogsServices
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly IRepository<TransactionLog, int> _transactionLogRepository;
        private readonly UserManager<User> userManager;
        public TransactionLogService(IRepository<TransactionLog, int> transactionLogRepository, UserManager<User> userManager)
        {
            _transactionLogRepository = transactionLogRepository;
            this.userManager = userManager;
        }

        private TransactionLog ConvertToBaseEntity(CreateTransactionLog createTransactionLog)
        {
            var transactionLog = new TransactionLog
            {
                //SenderID = createTransactionLog.SenderID,
                CampaignId = createTransactionLog.CampaignId,
                TransactionDate = createTransactionLog.TransactionDate,
                Amount = createTransactionLog.Amount,
                Description = createTransactionLog.Description,
                //BankId = createTransactionLog.BankId
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

        public async Task<bool> CreateTransactionLogAsync(TransactionLog createTransactionLog)
        {
            try
            {
                if (createTransactionLog == null)
                {
                    throw new Exception("CreateTransactionLog is null");
                }
                await _transactionLogRepository.InsertAsync(createTransactionLog);
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
            //SourceTransactionLog.SenderID = DesTransactionLog.SenderID;
            SourceTransactionLog.Amount = DesTransactionLog.Amount;
            //SourceTransactionLog.BankId = DesTransactionLog.BankId;
            SourceTransactionLog.Description = DesTransactionLog.Description;
            SourceTransactionLog.CampaignId = DesTransactionLog.CampaignId;
        }

        public async Task<TransactionLog> GetTransactionLogByCassoTransactionIdAsync(int id)
        {
            TransactionLog transactionLog = null!;
            try
            {
                transactionLog = await _transactionLogRepository.Get(x => x.CassoTransactionId == id);
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
        // lịch sử chuyển tiền vào chiến dịch
        public async Task<List<UserTransactionHistory>> GetUserTransactionsByCampaignIdAsync(int campaignId)
        {
            List<UserTransactionHistory> userTransaction = null!;
            try
            {
                userTransaction = await _transactionLogRepository.GetAll()
                   .Where(u => u.CampaignId == campaignId && u.Amount >= 0)
                   .Select(u => new UserTransactionHistory()
                   {
                       UserId = u.CreateQrCode.UserId,
                       CreateQrCodeId = u.CreateQrCode.Id,
                       IsPaid = u.CreateQrCode.IsPaid,
                       Amount = u.Amount,
                       CampaignId = u.CampaignId,
                       CampaignName = u.Campaign.Title,
                       Description = u.Description,
                       TransactionDate = u.TransactionDate,
                       tid = u.tid,
                   }).ToListAsync();
                if (userTransaction == null)
                {
                    throw new Exception("userTransaction not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetUserTransactionsByCampaignIdAsync: {ex.Message}");
            }

            return userTransaction;
        }

        public async Task<List<UserTransactionHistory>> GetUserTransactionsByUserIdAsync(string userId)
        {
            List<UserTransactionHistory> userTransaction = null!;
            try
            {
                userTransaction = await _transactionLogRepository.GetAll()
                   .Where(u => u.CreateQrCode.UserId.Equals(userId))
                   .Select(u => new UserTransactionHistory()
                   {
                       UserId = u.CreateQrCode.UserId,
                       CreateQrCodeId = u.CreateQrCode.Id,
                       IsPaid = u.CreateQrCode.IsPaid,
                       Amount = u.Amount,
                       CampaignId = u.CampaignId,
                       CampaignName = u.Campaign.Title,
                       Description = u.Description,
                       TransactionDate = u.TransactionDate,
                       tid = u.tid,
                   }).ToListAsync();
                if (userTransaction == null)
                {
                    throw new Exception("userTransaction not found");
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetUserTransactionsByUserIdAsync: {ex.Message}");
            }

            return userTransaction;
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

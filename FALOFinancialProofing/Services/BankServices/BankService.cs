using FALOFinancialProofing.DTOs.BankDTO;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services.CampaignService;
using Microsoft.EntityFrameworkCore;

namespace FALOFinancialProofing.Services.BankServices
{
    public class BankService : IBankService
    {
        private readonly IRepository<Bank, int> _bankRepository;
        private readonly ICampaignService _campaignService;

        public BankService(IRepository<Bank, int> bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<bool> CreateBankAsync(Bank createBank)
        {
            try
            {
                if (createBank == null)
                {
                    throw new Exception("CreateBank is null");
                }
                //var bank = ConvertToBaseEntity(createBank);
                await _bankRepository.InsertAsync(createBank);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateBank: {ex.Message}!");
            }

            return false;
        }

        private void ConvertToBaseEntity(Bank SourceBank, Bank DesBank)
        {
            SourceBank.Id = DesBank.Id;
            SourceBank.OwnerName = DesBank.OwnerName;
            SourceBank.AccountNumber = DesBank.AccountNumber;
            SourceBank.BankCodeName = DesBank.BankCodeName;
            SourceBank.acqId = DesBank.acqId;
            SourceBank.CassoAccountID = DesBank.CassoAccountID;
        }

        public async Task<BankInformation> GetBankByIdAsync(int id)
        {
            BankInformation bank = null!;
            try
            {
                var bankBase = await _bankRepository.Get(id);
                if (bankBase == null)
                {
                    throw new Exception("Bank not found");
                }
                bank = new BankInformation
                {
                    Id = bankBase.Id,
                    OwnerName = bankBase.OwnerName,
                    AccountNumber = bankBase.AccountNumber,
                    BankCodeName = bankBase.BankCodeName,
                    acqId = bankBase.acqId,
                    CassoAccountID = bankBase.CassoAccountID,
                };

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetBankByIdAsync: {ex.Message}");
            }

            return bank;
        }

        public async Task<IEnumerable<Bank>> GetAllBanksAsync()
        {
            List<Bank> data = null!;
            try
            {
                //.Include(bank => bank.Campaigns)
                data = await _bankRepository.GetAll().Where(bank => !bank.Campaigns.Any(c => c.BankId == bank.Id)).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllBanks: {ex.Message}");
            }

            return data;
        }

        public async Task<IEnumerable<Bank>> GetAllBanksInSystemAsync()
        {
            List<Bank> data = null!;
            try
            {
                //.Include(bank => bank.Campaigns)
                data = await _bankRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetAllBanksInSystemAsync: {ex.Message}");
            }

            return data;
        }
        // admin can update transaction logs
        public async Task<bool> UpdateBankAsync(Bank updateBank)
        {
            Bank bank = null!;
            bool result = false;
            try
            {
                bank = await _bankRepository.Get(updateBank.Id);
                if (bank == null)
                {
                    throw new Exception("Bank not found!");
                }
                ConvertToBaseEntity(bank, updateBank);
                result = await _bankRepository.UpdateAsync(bank);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateBank: {ex.Message}");
            }

            return result;
        }
        // admin can delete transaction logs
        public async Task<bool> DeleteBankAsync(int id)
        {
            Bank bank = null!;
            bool result = false;
            try
            {
                bank = await _bankRepository.Get(id);
                if (bank == null)
                {
                    throw new Exception("Bank not found!");
                }
                result = await _bankRepository.DeleteAsync(bank);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteBank: {ex.Message}");
            }

            return result;
        }

        // áp dụng cho mỗi chiến dịch chỉ có một tài khoản ngân hàng (có thể chưa dùng tới)
        public async Task<BankInformation> GetBankByCampaignIdAsync(int CampaignId)
        {
            BankInformation bank = null!;
            try
            {
                var bankBase = await _bankRepository.Get(b => b.Campaigns.Any(c => c.Id == CampaignId));
                if (bank == null)
                {
                    throw new Exception("Bank not found");
                }
                bank = new BankInformation
                {
                    Id = bankBase.Id,
                    OwnerName = bankBase.OwnerName,
                    AccountNumber = bankBase.AccountNumber,
                    BankCodeName = bankBase.BankCodeName,
                    acqId = bankBase.acqId,
                    CassoAccountID = bankBase.CassoAccountID,
                };

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetBankByCampaignIdAsync: {ex.Message}");
            }

            return bank;
        }

        public async Task<bool> SyncBankAccountsToBankDb(List<CassoBankAccount> cassoBankAccounts)
        {
            bool checkValid = false;
            try
            {
                var bankList = await GetAllBanksInSystemAsync(); //db
                foreach (var item in cassoBankAccounts)
                {
                    if (!bankList.Any(b => b.CassoAccountID == item.id))
                    {
                        var newBank = new Bank
                        {
                            OwnerName = item.accountName,
                            AccountNumber = item.accountNumber,
                            BankCodeName = item.bankCodeName,
                            acqId = item.BIN.Value,
                            CassoAccountID = item.id.Value
                        };
                        checkValid = await CreateBankAsync(newBank);
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"SyncBankAccountsToBankDb: {ex.Message}");
            }

            return checkValid;
        }

    }
}

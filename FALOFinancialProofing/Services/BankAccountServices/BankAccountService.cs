using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FALOFinancialProofing.Services.BankAccountServices
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FALOFinancialProofingDbContext _dbContext;

        public BankAccountService(IHttpClientFactory httpClientFactory, FALOFinancialProofingDbContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task<List<BankAccount>> GetAccounts()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Apikey AK_CS.5e3f98c0987011ef98eebd0598ac83dd.zKntSv5qWnmjiDZoy2xJJCRVs2g7Sqdk3XM4XvbfhPAla72SPbShdfMiSoEaIKYJQQBBvpMz");
                var response = await httpClient.GetAsync("https://oauth.casso.vn/v2/accounts");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<CassoApiResponseDTO>(jsonResponse);

                if (apiResponse.Error != 0)
                {
                    throw new Exception(apiResponse.Message);
                }

                if (apiResponse.Data == null)
                {
                    throw new Exception("Accounts list is null.");
                }

                // Lưu các tài khoản vào database
                foreach (var account in apiResponse.Data)
                {
                    // Kiểm tra nếu tài khoản đã tồn tại trong cơ sở dữ liệu
                    var existingAccount = await _dbContext.BankAccounts
                        .FirstOrDefaultAsync(a => a.AccountId == account.AccountId);

                    if (existingAccount == null)
                    {
                        // Thêm tài khoản mới
                        await _dbContext.BankAccounts.AddAsync(account);
                    }
                    else
                    {
                        // Cập nhật tài khoản hiện có
                        existingAccount.AccountNumber = account.AccountNumber;
                        existingAccount.AccountName = account.AccountName;
                        existingAccount.BankCode = account.BankCode;
                        existingAccount.Balance = account.Balance;
                    }
                }

                await _dbContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                return apiResponse.Data; // Trả về danh sách tài khoản
            }
        }
    }
}

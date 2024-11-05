using Newtonsoft.Json;
using System.Collections.Generic;

namespace FALOFinancialProofing.FALOHomePage.Services
{
    public class TransactionPollingDirect
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TransactionPollingDirect(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<BankTransaction>> GetTransactionAsync()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Apikey AK_CS.5e3f98c0987011ef98eebd0598ac83dd.zKntSv5qWnmjiDZoy2xJJCRVs2g7Sqdk3XM4XvbfhPAla72SPbShdfMiSoEaIKYJQQBBvpMz");
                var response = await httpClient.GetAsync("https://oauth.casso.vn/v2/accounts/9957/transactions");

                // Ensure the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize into the updated wrapper class
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                // Check for errors in the response
                if (apiResponse.Error != 0)
                {
                    throw new Exception(apiResponse.Message); // Handle the error
                }

                

                List <BankTransaction> transactionList = new List<BankTransaction>();

                foreach (BankTransaction r in apiResponse.Data.Records)
                {
                    if(r != null && r.Amount > 0)
                    {
                        transactionList.Add(r);
                    }
                }
                return transactionList; // Return the list of transactions
            }
        }
    }

    public class ApiResponseTesting
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public List<BankAccount> Data { get; set; }
    }


    public class ApiResponse
    {
        public int Error { get; set; }
        public string Message { get; set; }
        public TransactionData Data { get; set; }
    }

    public class TransactionData
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<BankTransaction> Records { get; set; }
    }

    public class BankTransaction
    {
        public int Id { get; set; }
        public string Tid { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal? CusumBalance { get; set; }
        public DateTime When { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BankSubAccId { get; set; }
        public string PaymentChannel { get; set; }
        public string VirtualAccount { get; set; }
        public string VirtualAccountName { get; set; }
        public string CorresponsiveName { get; set; }
        public string CorresponsiveAccount { get; set; }
        public string CorresponsiveBankId { get; set; }
        public string CorresponsiveBankName { get; set; }
        public int AccountId { get; set; }
        public string BankCodeName { get; set; }
    }

    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal? Balance { get; set; }
        public string Currency { get; set; }
        public string Swift { get; set; }
        public string Citad { get; set; }
        public string ServiceType { get; set; }
        public string BankName { get; set; }
        public int Bin { get; set; }
        public string BankCodeName { get; set; }
        public string Memo { get; set; }
        public int ConnectStatus { get; set; }
        public DateTime BeginningSettingDate { get; set; }
        public DateTime? BeginningTxnDate { get; set; } // Nullable
        public decimal BeginningBalance { get; set; }
        public decimal CreditTxnTotal { get; set; }
        public decimal CreditTxnAmount { get; set; }
        public decimal DebitTxnTotal { get; set; }
        public decimal DebitTxnAmount { get; set; }
        public DateTime? LockSyncDate { get; set; } // Nullable
        public decimal? EndingBalance { get; set; }
        public DateTime EndingTxnDate { get; set; }
    }
}

//using Newtonsoft.Json;
//using System.Collections.Generic;

//namespace FALOFinancialProofing.FALOHomePage.Services
//{
//    public class BankAccountService
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        public BankAccountService(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }

//        public async Task<List<BankAccount>> GetAccounts()
//        {
//            using (var httpClient = _httpClientFactory.CreateClient())
//            {
//                httpClient.DefaultRequestHeaders.Add("Authorization", "Apikey");
//                var response = await httpClient.GetAsync("https://oauth.casso.vn/v2/accounts");

//                // Ensure the response is successful
//                if (!response.IsSuccessStatusCode)
//                {
//                    var errorContent = await response.Content.ReadAsStringAsync();
//                    throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
//                }

//                var jsonResponse = await response.Content.ReadAsStringAsync();

//                // Deserialize into the updated wrapper class
//                var apiResponse = JsonConvert.DeserializeObject<ApiResponseTesting>(jsonResponse);

//                // Check for errors in the response
//                if (apiResponse.Error != 0)
//                {
//                    throw new Exception(apiResponse.Message); // Handle the error
//                }

//                if (apiResponse.Data == null)
//                {
//                    throw new Exception("Accounts list is null.");
//                }

//                return apiResponse.Data; // Return the list of accounts
//            }
//        }
//    }
//    public class ApiResponseTesting
//    {
//        public int Error { get; set; }
//        public string Message { get; set; }
//        public List<BankAccount> Data { get; set; }
//    }

//    public class BankAccount
//    {
//        public int Id { get; set; }
//        public string AccountNumber { get; set; }
//        public string AccountName { get; set; }
//        public string AccountType { get; set; }
//        public decimal? Balance { get; set; }
//        public string Currency { get; set; }
//        public string Swift { get; set; }
//        public string Citad { get; set; }
//        public string ServiceType { get; set; }
//        public string BankName { get; set; }
//        public int Bin { get; set; }
//        public string BankCodeName { get; set; }
//        public string Memo { get; set; }
//        public int ConnectStatus { get; set; }
//        public DateTime BeginningSettingDate { get; set; }
//        public DateTime? BeginningTxnDate { get; set; } // Nullable
//        public decimal BeginningBalance { get; set; }
//        public decimal CreditTxnTotal { get; set; }
//        public decimal CreditTxnAmount { get; set; }
//        public decimal DebitTxnTotal { get; set; }
//        public decimal DebitTxnAmount { get; set; }
//        public DateTime? LockSyncDate { get; set; } // Nullable
//        public decimal? EndingBalance { get; set; }
//        public DateTime EndingTxnDate { get; set; }
//    }
//}

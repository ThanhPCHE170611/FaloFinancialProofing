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

        public async Task<List<TransactionDetails>> GetTransactionAsync(int id)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Apikey ");
                var response = await httpClient.GetAsync($"https://oauth.casso.vn/v2/accounts/{id}/transactions");

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

                

                List <TransactionDetails> transactionList = new List<TransactionDetails>();

                foreach (TransactionDetails r in apiResponse.Data.Records)
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
        public List<TransactionDetails> Records { get; set; }
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

    public class TransactionDetails
    {
        public int PrivateId { get; set; }
        public string Reference { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public long? RunningBalance { get; set; }
        public string VirtualAccountNumber { get; set; }
        public string VirtualAccountName { get; set; }
        public string PaymentChannel { get; set; }
        public string CounterAccountNumber { get; set; }
        public string CounterAccountName { get; set; }
        public string CounterAccountBankId { get; set; }
        public string CounterAccountBankName { get; set; }
    }
}

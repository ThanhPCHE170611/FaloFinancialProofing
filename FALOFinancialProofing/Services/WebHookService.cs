using FALOFinancialProofing.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public class WebHookService
    {
        private readonly HttpClient _httpClient;
        private readonly string APIKey;
        private readonly string SynUrl = "https://oauth.casso.vn/v2/sync";
        private readonly string WebhookUrl = "https://oauth.casso.vn/v2/webhooks";
        private readonly string AccountNumberUrl = "https://oauth.casso.vn/v2/accounts";

        public WebHookService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            APIKey = configuration.GetSection("Authentication:Casso:Apikey").Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", APIKey);

        }
        public async Task<SyncResponse> TransactionSync(SyncRequest request)
        {
            SyncResponse syncResponse = null!;
            try
            {

                var jsonData = JsonConvert.SerializeObject(request);
                HttpContent httpContent = new StringContent(jsonData, Encoding.ASCII, "application/json");
                var response = await _httpClient.PostAsync(SynUrl, httpContent);
                string content = await response.Content.ReadAsStringAsync();
                syncResponse = JsonConvert.DeserializeObject<SyncResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"TransactionSync: {ex.Message}");
            }
            return syncResponse;
        }


        public async Task<RootResponse> CreateWebHookAsync(WebHookDataRequest webHookDataRequest)
        {
            RootResponse rootResponse = null;
            try
            {
                var jsonData = JsonConvert.SerializeObject(webHookDataRequest);
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://oauth.casso.vn/v2/webhooks", httpContent);
                string content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<RootResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateWebHookAsync: {ex.Message}");
            }

            return rootResponse;
        }
        public async Task<AccountNumberResponse> GetAllBankAccountNumberAsync()
        {
            AccountNumberResponse rootResponse = null!;
            try
            {
                var response = await _httpClient.GetAsync($"{AccountNumberUrl}");
                var content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<AccountNumberResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetWebHookDetailByIdAsync: {ex.Message}");

            }
            return rootResponse;
        }
        public async Task<RootResponse> GetWebHookDetailByIdAsync(int id)
        {
            RootResponse rootResponse = null!;
            try
            {
                var response = await _httpClient.GetAsync($"{WebhookUrl}/{id}");
                var content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<RootResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetWebHookDetailByIdAsync: {ex.Message}");

            }
            return rootResponse;
        }

        public async Task<RootResponse> UpdateWebHookAsync(int id, WebHookDataRequest webHookDataRequest)
        {
            RootResponse rootResponse = null!;
            try
            {
                var jsonData = JsonConvert.SerializeObject(webHookDataRequest);
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{WebhookUrl}/{id}", httpContent);
                var content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<RootResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateWebHookAsync: {ex.Message}");

            }
            return rootResponse;
        }
        public async Task<RootResponse> DeleteWebHookByIdAsync(int id)
        {
            RootResponse rootResponse = null!;
            try
            {
                var response = await _httpClient.DeleteAsync($"{WebhookUrl}/{id}");
                var content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<RootResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteWebHookByIdAsync: {ex.Message}");
            }

            return rootResponse;
        }
        public async Task<RootsResponse> DeleteAllWebHooksByUrlAsync(string webhook)
        {
            RootsResponse rootsResponse = null!;
            try
            {
                var response = await _httpClient.DeleteAsync($"{WebhookUrl}?webhook={webhook}");
                var content = await response.Content.ReadAsStringAsync();
                rootsResponse = JsonConvert.DeserializeObject<RootsResponse>(content);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteAllWebHooksByUrlAsync: {ex.Message}");
            }

            return rootsResponse;
        }
    }
    public class WebHookDataRequest
    {
        public string webhook { get; set; }
        public string secure_token { get; set; }
        public bool income_only { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WebHookDataResponse
    {
        public int id { get; set; }
        public string channel { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public int send_only_income { get; set; }
    }

    public class RootResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public WebHookDataResponse data { get; set; }
    }
    public class SyncResponseData
    {
        public int id { get; set; }
        public string message { get; set; }
    }
    public class SyncResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public List<SyncResponseData> data { get; set; }
    }

    public class RootsResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public List<WebHookDataResponse> data { get; set; }
    }
    public class SyncRequest
    {
        public string bank_acc_id { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AccountNumber
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public int balance { get; set; }
        public string currency { get; set; }
        public string swift { get; set; }
        public string citad { get; set; }
        public string serviceType { get; set; }
        public string bankName { get; set; }
        public int BIN { get; set; }
        public string bankCodeName { get; set; }
        public string memo { get; set; }
        public int connectStatus { get; set; }
        public string beginningSettingDate { get; set; }
        public object beginningTxnDate { get; set; }
        public int beginningBalance { get; set; }
        public int creditTxnTotal { get; set; }
        public int creditTxnAmount { get; set; }
        public int debitTxnTotal { get; set; }
        public int debitTxnAmount { get; set; }
        public object lockSyncDate { get; set; }
        public int endingBalance { get; set; }
        public DateTime endingTxnDate { get; set; }
    }

    public class AccountNumberResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public List<AccountNumber> data { get; set; }
    }


}

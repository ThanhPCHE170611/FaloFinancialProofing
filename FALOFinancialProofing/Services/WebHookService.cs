using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FALOFinancialProofing.Services
{
    public class WebHookService
    {
        private readonly HttpClient _httpClient;
        private string APIKey = "AK_CS.fdd986909e6d11ef983e076fcd8dbc01.GDvaZx4xSN2fUXUFDNDQExfE3XkApz8gMd3lkWZGImxvcT0qZpOaxxu8TUgvxE2PAunnrkTH";
        private string SynUrl = "https://oauth.casso.vn/v2/sync";
        private string WebhookUrl = "https://oauth.casso.vn/v2/webhooks";

        public WebHookService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", APIKey);
        }
        public async Task<bool> TransactionSync(string bank_acc_id)
        {
            var checkValid = true;
            try
            {
                HttpContent httpContent = new StringContent(bank_acc_id);
                var response = await _httpClient.PostAsync(SynUrl, httpContent);
            }
            catch (Exception ex)
            {
                checkValid = false;
                await Console.Out.WriteLineAsync($"TransactionSync: {ex.Message}");
            }
            return checkValid;
        }


        public async Task CreateWebHookAsync(WebHookDataRequest webHookDataRequest)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(webHookDataRequest);
                HttpContent httpContent = new StringContent(jsonData);
                var response = await _httpClient.PostAsync(SynUrl, httpContent);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CreateWebHookAsync: {ex.Message}");

            }
        }

        public async Task GetWebHookDetailByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{SynUrl}/{id}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"GetWebHookDetailByIdAsync: {ex.Message}");

            }
        }

        public async Task UpdateWebHookAsync(int id, WebHookDataRequest webHookDataRequest)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(webHookDataRequest);
                HttpContent httpContent = new StringContent(jsonData);
                var response = await _httpClient.PutAsync($"{SynUrl}/{id}", httpContent);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"UpdateWebHookAsync: {ex.Message}");

            }
        }
        public async Task DeleteWebHookAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{SynUrl}/{id}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteWebHookAsync: {ex.Message}");
            }
        }
        public async Task DeleteAllWebHooksByUrlAsync(string webhook)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{SynUrl}?webhook={webhook}");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"DeleteAllWebHooksByUrlAsync: {ex.Message}");
            }
        }
    }
    public class WebHookDataRequest
    {
        public string webhook { get; set; }
        public string secure_token { get; set; }
        public bool income_only { get; set; }
    }

}

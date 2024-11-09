using System.Net.Http.Headers;

namespace FALOFinancialProofing.Services
{
    public class WebHookService
    {
        private readonly HttpClient _httpClient;
        private string APIKey = "AK_CS.fdd986909e6d11ef983e076fcd8dbc01.GDvaZx4xSN2fUXUFDNDQExfE3XkApz8gMd3lkWZGImxvcT0qZpOaxxu8TUgvxE2PAunnrkTH";
        private string SynUrl = "https://oauth.casso.vn/v2/sync";
        public WebHookService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
        }
        public async Task TransactionSync(string bank_acc_id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", APIKey);
                HttpContent httpContent = new StringContent(bank_acc_id);
                var response = await _httpClient.PostAsync(SynUrl, httpContent);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);

            }
        }
    }
}

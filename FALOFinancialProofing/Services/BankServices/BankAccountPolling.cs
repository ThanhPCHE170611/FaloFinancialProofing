
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FALOFinancialProofing.Services.BankServices
{
    public class BankAccountPolling : BackgroundService
    {

        private const int PollingInterval = 60000; // 1 minute
        private static string APIKey;
        private const string API_GET = "https://oauth.casso.vn/v2/accounts";
        private static IBankService _bankService;
        private static IServiceScopeFactory _serviceScopeFactory;
        //public BankAccountPolling(IConfiguration configuration, IBankService bankService)
        //{
        //    APIKey = configuration.GetSection("Authentication:Casso:Apikey").Value;
        //    _bankService = bankService;
        //}
        public BankAccountPolling(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            APIKey = configuration.GetSection("Authentication:Casso:Apikey").Value;
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckPaid(stoppingToken);
                await Task.Delay(PollingInterval, stoppingToken);
            }
        }

        private async Task CheckPaid(CancellationToken stoppingToken)
        {
            using (var httpClient = new HttpClient())
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _bankService = scope.ServiceProvider.GetRequiredService<IBankService>();
                    _bankService.GetBankByIdAsync(1);
                    // Sử dụng bankService để thực hiện công việc
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", APIKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await httpClient.GetAsync(API_GET, stoppingToken);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<AccountResponse>(jsonResponse);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }
    }

    public class CassoBankAccount
    {
        public int id { get; set; }
        public string accountNumber { get; set; }
        public string accountName { get; set; }
        public string accountType { get; set; }
        public int balance { get; set; }
        public string beginningSettingDate { get; set; }
        public DateTime beginningTxnDate { get; set; }
        public int beginningBalance { get; set; }
        public int creditTxnTotal { get; set; }
        public int creditTxnAmount { get; set; }
        public int debitTxnTotal { get; set; }
        public int debitTxnAmount { get; set; }
        public object lockSyncDate { get; set; }
        public int endingBalance { get; set; }
        public DateTime endingTxnDate { get; set; }
        public string currency { get; set; }
        public string swift { get; set; }
        public string citad { get; set; }
        public string serviceType { get; set; }
        public string bankName { get; set; }
        public string bankCodeName { get; set; }
        public string memo { get; set; }
        public int connectStatus { get; set; }
        public int BIN { get; set; }
    }

    public class AccountResponse
    {
        public int error { get; set; }
        public string message { get; set; }
        public List<CassoBankAccount> data { get; set; }
    }
}


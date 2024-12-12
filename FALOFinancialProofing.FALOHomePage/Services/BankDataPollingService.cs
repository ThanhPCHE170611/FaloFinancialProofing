//namespace FALOFinancialProofing.FALOHomePage.Services
//{
//    using Microsoft.Extensions.Hosting;
//    using System.Net.Http.Headers;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using Newtonsoft.Json;

//    public class BankDataPollingService : IHostedService, IDisposable
//    {
//        private Timer _timer;
//        private const int PollingInterval = 60000;

//        private const string API_KEY = "";
//        private const string API_GET = "https://oauth.casso.vn/v2/transactions";
//        public void Dispose()
//        {
//            _timer?.Dispose();
//        }

//        public Task StartAsync(CancellationToken cancellationToken)
//        {
//            _timer = new Timer(CheckPaid, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(PollingInterval));
//            return Task.CompletedTask;
//        }

//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            _timer?.Change(Timeout.Infinite, 0);
//            return Task.CompletedTask;
//        }

//        private static async void CheckPaid(object state)
//        {
//            using (var httpClient = new HttpClient())
//            {
//                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Apikey", API_KEY);
//                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//                try
//                {
//                    var response = await httpClient.GetAsync(API_GET);
//                    response.EnsureSuccessStatusCode();

//                    var jsonResponse = await response.Content.ReadAsStringAsync();
//                    var data = JsonConvert.DeserializeObject(jsonResponse);
//                    Console.WriteLine(JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented));
//                }
//                catch (HttpRequestException e)
//                {
//                    Console.WriteLine($"Request error: {e.Message}");
//                }
//            }
//        }
//    }

//    public class Transaction
//    {
//        public string Log_Id { get; set; }
//        public string TransactionID { get; set; }
//        public DateTime TransactionDate {  get; set; }
//        public decimal Amount { get; set; }
//        public string Description {  get; set; }
        
//    }
//}

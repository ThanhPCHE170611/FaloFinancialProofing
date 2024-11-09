using FALOFinancialProofing.Helpers;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;

namespace FALOFinancialProofing.Services
{
    public class BankService1
    {
        private readonly HttpClient _httpClient;

        public BankService1(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyHttpClient");
        }
        public async Task<Bank> GetBanks()
        {
            Bank bank = new Bank();
            var response = await _httpClient.GetAsync("https://api.vietqr.io/v2/banks");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                bank = JsonConvert.DeserializeObject<Bank>(content);
            }
            return bank;
        }

        public async Task<BankResponse> GetQRCode(BankRequest bankRequest)
        {
            var dataResponse = new BankResponse();
            string jsonData = JsonConvert.SerializeObject(bankRequest);
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.vietqr.io/v2/generate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dataResponse = JsonConvert.DeserializeObject<BankResponse>(content);
            }
            return dataResponse;
        }

        public byte[] ConvertBase64ToImage(string base64String)
        {
            // Loại bỏ tiền tố "data:image/png;base64," nếu có
            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1];
            }

            // Chuyển đổi chuỗi Base64 thành mảng byte
            byte[] imageBytes = Convert.FromBase64String(base64String);

            //// Tạo đối tượng MemoryStream từ mảng byte
            //using (var ms = new MemoryStream(imageBytes))
            //{
            //    // Tạo đối tượng Image từ MemoryStream
            //    return Image.FromStream(ms);
            //}
            return imageBytes;
        }
    }
}

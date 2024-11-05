using Newtonsoft.Json;
using System.Collections.Generic;

namespace FALOFinancialProofing.FALOHomePage.Services
{
    public class BankAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BankAccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<BankAccount>> GetAccounts()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Apikey AK_CS.5e3f98c0987011ef98eebd0598ac83dd.zKntSv5qWnmjiDZoy2xJJCRVs2g7Sqdk3XM4XvbfhPAla72SPbShdfMiSoEaIKYJQQBBvpMz");
                var response = await httpClient.GetAsync("https://oauth.casso.vn/v2/accounts");

                // Ensure the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize into the updated wrapper class
                var apiResponse = JsonConvert.DeserializeObject<ApiResponseTesting>(jsonResponse);

                // Check for errors in the response
                if (apiResponse.Error != 0)
                {
                    throw new Exception(apiResponse.Message); // Handle the error
                }

                if (apiResponse.Data == null)
                {
                    throw new Exception("Accounts list is null.");
                }

                return apiResponse.Data; // Return the list of accounts
            }
        }
    }
    }

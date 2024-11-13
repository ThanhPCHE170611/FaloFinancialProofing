using FALOFinancialProofing.FALOHomePage.Models;
using FALOFinancialProofing.FALOHomePage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    
    public class DonationController : Controller
    {
        // GET: DonationController
        private readonly TransactionPollingDirect _transactionPollingDirect;
        private readonly BankAccountService bankAccountService;

        private readonly IHttpClientFactory _httpClientFactory;

        public DonationController(TransactionPollingDirect transactionPollingDirect, BankAccountService bankData, IHttpClientFactory httpClientFactory)
        {
            _transactionPollingDirect = transactionPollingDirect;
            bankAccountService = bankData;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index(int id)
        {
            //var transactions = await _transactionPollingDirect.GetTransactionAsync(id);
            //return View(transactions);
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            else
            {
                try
                {
                    // Get HttpClient from the factory
                    var client = _httpClientFactory.CreateClient();

                    // Make a GET request to the API
                    HttpResponseMessage response = await client.GetAsync($"https://localhost:7294/api/TransactionLogs/GetCampaignTransactionLogs/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        var transactionLogsResponse = JsonConvert.DeserializeObject<TransactionDTO.TransactionLogsResponse>(jsonResponse);

                        ViewBag.Transactions = transactionLogsResponse.Data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to fetch transaction logs.";
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error404", "Error");
                }

                return View();
             }
        }

        public async Task<IActionResult> ViewAccounts()
        {
            var accounts = await bankAccountService.GetAccounts();
            return View(accounts);
        }

        //Validate whether user has logged in yet
        public async Task<IActionResult> CreateQR(int campaignId)
        {
            if (campaignId == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            else
            {
                try
                {
                    // Get HttpClient from the factory
                    var client = _httpClientFactory.CreateClient();

                    // Make a GET request to the API
                    var response = await client.GetStringAsync($"https://localhost:7294/api/Campaign/GetCampaignDetailsById/{campaignId}");

                    // Deserialize the JSON response into an object
                    var campaignDetails = JsonConvert.DeserializeObject<ApiResponseCampaignDetails>(response);

                    if (campaignDetails == null)
                    {
                        return RedirectToAction("Error404", "Error");
                    }

                    ViewBag.campaignDetails = campaignDetails.Data;

                    //Validate whether user has logged in before



                    //Create ViewBag for CampaignId, BankID and user id
                    ViewBag.donationDetails = new QRDTO("1cf471eb-03d6-40f3-ae97-85c16d42c475", 0, campaignDetails.Data.BankId, campaignId);

                    //Send User To Page with CampaignId, BankID and user id
                    return View();


                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error404", "Error");
                }

            }
        }

        public async Task<IActionResult> DownloadAccountingBooks(int campaignId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var firstApiUrl = $"https://localhost:7294/api/AccountingBook/getaccountingbookincampaign?campaignId={campaignId}";

                var firstApiResponse = await client.GetStringAsync(firstApiUrl);

                var firstApiResult = JsonConvert.DeserializeObject<ApiResponse>(firstApiResponse);

                if (!firstApiResult.Success)
                {
                    return RedirectToAction("Error404", "Error");

                }

                var filePath = firstApiResult.Data.FilePath;

                var secondApiUrl = $"https://localhost:7294/api/AccountingBook/downloadaccountingbook/{filePath}";

                var secondApiResponse = await client.GetAsync(secondApiUrl);

                if (secondApiResponse.IsSuccessStatusCode)
                {
                    var fileContent = await secondApiResponse.Content.ReadAsByteArrayAsync();
                    var fileName = filePath; 

                    return File(fileContent, "application/octet-stream", fileName);
                }
                else
                {
                    return Content("Failed to download the accounting book.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Content($"Error: {ex.Message}");
            }
        }
        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public ApiResponseData Data { get; set; }
        }

        public class ApiResponseData
        {
            public string FilePath { get; set; }
            public int CampaignId { get; set; }
            public string CampaignName { get; set; }
            public int Id { get; set; }
        }

        // GET: DonationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DonationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DonationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

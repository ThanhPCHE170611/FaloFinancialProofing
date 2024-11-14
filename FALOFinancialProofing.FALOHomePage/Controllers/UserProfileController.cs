using FALOFinancialProofing.FALOHomePage.Models;
using FALOFinancialProofing.FALOHomePage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfileController
        private readonly IHttpClientFactory _httpClientFactory;

        public UserProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Profile()
        {
            return View();
        }

        public async Task<IActionResult> DonationHistory(int currentPage = 1)
        {
            try
            {
                // Get HttpClient from the factory
                var client = _httpClientFactory.CreateClient();

                string userId = HttpContext.Session.GetString("UserId");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Authentication");
                }

                // Make a GET request to the API
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7294/api/TransactionLogs/GetUserTransactionLogs/{userId}?currentPage=1");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var transactionLogsResponse = JsonConvert.DeserializeObject<TransactionDTO.TransactionLogResponse>(jsonResponse);

                    ViewBag.Transactions = transactionLogsResponse.Data;

                    ViewBag.CurrentPage = transactionLogsResponse.Data.CurrentPage;
                    ViewBag.TotalPages = (int)Math.Ceiling((double)transactionLogsResponse.Data.DataCount / 10);
                    ViewBag.DataCount = transactionLogsResponse.Data.DataCount;
                    ViewBag.PageSize = 10;
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

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Homepage");
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
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

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfileController/Edit/5
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

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProfileController/Delete/5
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

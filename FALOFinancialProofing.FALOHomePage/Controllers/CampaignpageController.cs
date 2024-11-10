using FALOFinancialProofing.FALOHomePage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class CampaignpageController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public CampaignpageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // GET: CampaignpageController
        public async Task<IActionResult> Index(int? id)
        {
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
                    var response = await client.GetStringAsync($"https://localhost:7294/api/Campaign/GetCampaignDetailsById/{id}");

                    // Deserialize the JSON response into an object
                    var campaignDetails = JsonConvert.DeserializeObject<ApiResponseCampaignDetails>(response);

                    if(campaignDetails == null)
                    {
                        return RedirectToAction("Error404", "Error");
                    }

                    ViewBag.campaignDetails = campaignDetails.Data;

                    // Pass the ViewModel to the view
                    return View();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error404", "Error");
                }

            }
        }

        public async Task<IActionResult> SpendingProof(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();

                    // Make a GET request to the API
                    var response = await client.GetStringAsync($"https://localhost:7294/api/Campaign/GetCampaignDetailsById/{id}");

                    var campaignDetails = JsonConvert.DeserializeObject<ApiResponseCampaignDetails>(response);

                    ViewBag.campaignDetails = campaignDetails.Data;

                    return View();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error404", "Error");
                }

            }
        }


        // GET: CampaignpageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampaignpageController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ListPageRedirect()
        {
            return View("CampaignList");
        }

        // POST: CampaignpageController/Create
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

        // GET: CampaignpageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampaignpageController/Edit/5
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

        // GET: CampaignpageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaignpageController/Delete/5
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

using FALOFinancialProofing.FALOHomePage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class ProjectpageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProjectpageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // GET: ProjectpageController
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                try
                {
                    // Get HttpClient from the factory
                    var client = _httpClientFactory.CreateClient();

                    // Make a GET request to the API
                    var response = await client.GetStringAsync($"https://localhost:7294/api/Projects/GetProjectDetailsById/{id}");

                    // Deserialize the JSON response into an object
                    var projectDetails = JsonConvert.DeserializeObject<ApiResponseProject>(response);

                    string url = "https://localhost:7294/api/Campaign/GetAllCampaignByProjectId/" + id +"?currentPage=1";
                    // Get all campaigns through project id
                    var responseCampaigns = await client.GetStringAsync(url);
                    var campaigns = JsonConvert.DeserializeObject<ApiResponseCampaign>(responseCampaigns);

                    // Check if campaigns data is valid
                    if (campaigns?.Data?.Data == null)
                    {
                        // Handle the case where there are no campaigns or invalid data
                        campaigns.Data = new CampaignData { Data = new List<CampaignDTO>() }; // Provide an empty list
                    }

                    ViewBag.projectDetails = projectDetails.Data;
                    ViewBag.campaigns = campaigns.Data.Data;

                    // Pass the ViewModel to the view
                    return View();
                }
                catch (Exception ex)
                {
                    return View();
                }

            }
        }

        // GET: ProjectpageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult ListPageRedirect()
        {
            return View("ProjectList");
        }

        // GET: ProjectpageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectpageController/Create
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

        // GET: ProjectpageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectpageController/Edit/5
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

        // GET: ProjectpageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectpageController/Delete/5
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

using FALOFinancialProofing.FALOHomePage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static FALOFinancialProofing.FALOHomePage.Models.AttachmentFileDTO;

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

        public async Task<IActionResult> CampaignActivity(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error404", "Error");
            }
            else
            {
                ViewBag.campaignId = id;
                return View();
                //try
                //{
                //    var client = _httpClientFactory.CreateClient();

                //    // Make a GET request to the API
                //    HttpResponseMessage response = await client.GetAsync($"https://localhost:7294/api/AttachmentFile/getallpaymentattachmentincampaignwithrequest/{id}?page={page}");

                //    if (response.IsSuccessStatusCode)
                //    {
                //        string jsonResponse = await response.Content.ReadAsStringAsync();

                //        var attachmentFileResponse = JsonConvert.DeserializeObject<AttachmentFileResponse>(jsonResponse);

                //        if (attachmentFileResponse != null && attachmentFileResponse.Success && attachmentFileResponse.Data != null && attachmentFileResponse.Data.Data.Any())
                //        {
                //            ViewBag.Attachments = attachmentFileResponse.Data.Data;
                //            ViewBag.Message = attachmentFileResponse.Message;
                //        }
                //        else
                //        {
                //            ViewBag.ErrorMessage = attachmentFileResponse?.Message ?? "Attachment files not found";
                //        }
                //    }
                //    else
                //    {
                //        // API call failed
                //        ViewBag.ErrorMessage = "Failed to fetch attachment files.";
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return RedirectToAction("Error404", "Error");
                //}
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

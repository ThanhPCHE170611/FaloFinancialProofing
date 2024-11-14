using FALOFinancialProofing.FALOHomePage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Prepare the data for the API call
            var loginData = new
            {
                Username = username,
                Password = password
            };

            // Create the HttpClient instance
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "https://localhost:7294/api/Users/Login";

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            // Call the API
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<LoginDTO.ApiResponse>(responseData);

                if (apiResponse.Success)
                {
                    var token = apiResponse.Data.AccessToken;
                    var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var userId = decodedToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    HttpContext.Session.SetString("JwtToken", token);
                    HttpContext.Session.SetString("UserId", userId.ToString());
                    return RedirectToAction("Index", "Homepage");
                }
                else
                {
                    // If login failed, return error message
                    ViewBag.ErrorMessage = apiResponse.Message;
                    return View("Login");
                }
            }

            // If API call fails
            ViewBag.ErrorMessage = "An error occurred while contacting the server. Please try again.";
            return View("Login");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        // GET: AuthenticationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: AuthenticationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthenticationController/Create
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

        // GET: AuthenticationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthenticationController/Edit/5
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

        // GET: AuthenticationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthenticationController/Delete/5
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

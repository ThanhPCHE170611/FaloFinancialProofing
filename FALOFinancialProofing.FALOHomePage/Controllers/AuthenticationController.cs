using FALOFinancialProofing.FALOHomePage.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
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

        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties() { RedirectUri = Url.Action("GoogleResponse") });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var accessToken = result.Properties.GetTokenValue("access_token");
            if (accessToken == null)
                return BadRequest("accessToken is null");
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new { claim.Issuer, claim.OriginalIssuer, claim.Type, claim.Value });
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("AccessToken", accessToken);
            var response = await client.PostAsync("https://localhost:7294/api/Users/Login-Google/Donor", null);
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);
            //ViewData["serverAccessToken"] = apiResponse != null ? apiResponse.Data.AccessToken : null;
            if (apiResponse == null || apiResponse.Success == false)
            {
                return RedirectToAction(nameof(Login), new { ErrorMessage = apiResponse != null ? apiResponse.Message : null });
            }

            var token = apiResponse.Data.AccessToken;
            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userId = decodedToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            HttpContext.Session.SetString("JwtToken", token);
            HttpContext.Session.SetString("UserId", userId.ToString());

            return RedirectToAction("Index", "Homepage");
        }

        public Dictionary<string, List<string>> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var tokenClaims = jsonToken.Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(group => group.Key, group => group.Select(claim => claim.Value).ToList());
            return tokenClaims;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string? ErrorMessage)
        {
            ViewBag.ErrorMessage = ErrorMessage;
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
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TokenModel Data { get; set; }
    }
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
    }
}

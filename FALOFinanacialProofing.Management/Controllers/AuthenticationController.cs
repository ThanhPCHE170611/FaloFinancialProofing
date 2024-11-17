using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using FALOFinanacialProofing.Management.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FALOFinanacialProofing.Management.Controllers
{
    public class AuthenticationController : Controller
    {
        public ActionResult Login()
        {
            return View();
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
            var response = await client.PostAsync("https://localhost:7294/api/Users/Login-Google/Volunteer", null);
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseString);
            //ViewData["serverAccessToken"] = apiResponse != null ? apiResponse.Data.AccessToken : null;
            if (apiResponse == null || apiResponse.Success == false)
            {
                return RedirectToAction(nameof(Login));
            }
            // sau khi decode nếu thành công kiểm tra nếu role có một thì cho chuyển vào home page luôn còn không thì sẽ chuyển vào trang chọn role// 
            var decodedValues = DecodeJwtToken(apiResponse.Data.AccessToken);
            if (decodedValues["RoleId"].Count == 1)
            {
                // chuyển vào home page
                string a = apiResponse.Data.AccessToken;
                return RedirectToAction("Index", "Home", new { AccessToken = apiResponse.Data.AccessToken });
            }
            // trang chọn role
            return RedirectToAction(nameof(ChooseRole), new { AccessToken = apiResponse.Data.AccessToken });
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
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ChooseRole(string AccessToken)
        {
            return View((object)AccessToken);
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

using FALOFinancialProofing.Attributes;
using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FALOFinancialProofing.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly BankService1 bankService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, BankService1 bankService)
        {
            _logger = logger;
            this.bankService = bankService;
        }


        [HttpGet(Name = "GetWeatherForecast")]
        // Must use Policy that is defined in Startup.cs else error
        //[MinimumAge(21)]
        //[Authorize(Policy = "MinimumAge22")]
        //[MinimumAgeAuthorize(22)]
        //[Authorize(Roles = "User")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Policy = "Admin")]
        //[Authorize]
        [RoleAttribute(AppRole.ProjectManagementBoard)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        //[Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme)]
        //[HttpGet("{id}")]
        //public IEnumerable<WeatherForecast> Get(int id)
        //{
        //    var user = this.User;
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        [HttpGet("TestSetSession/{name}")]
        public IEnumerable<WeatherForecast> GetA(string name)
        {
            HttpContext.Session.SetString("name", name);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("TestGetSession")]
        public IEnumerable<WeatherForecast> GetB()
        {
            var name = HttpContext.Session.GetString("name");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        //[HttpGet("ReturnImage")]
        //public async Task<IActionResult> GetImageByAPI()
        //{
        //    var filePath = "E:\\PRN231\\PRNPEImage\\ACB.png";
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound();
        //    }

        //    var image = System.IO.File.OpenRead(filePath);
        //    return File(image, "image/png");
        //}

        [HttpGet("Get-Banks")]
        public async Task<IActionResult> GetBanks()
        {
            //if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            //{
            //    var token = HttpContext.Request.Headers["Authorization"].ToString();
            //    var banks = await bankService.GetBanks();
            //    return Ok(banks);
            //}
            var b = await bankService.GetBanks();
            BankRequest bankRequest = new BankRequest()
            {
                accountNo = "1016161976",
                accountName = "Nguyen Van Duc",
                acqId = 970436,
                amount = 1041321,
                addInfo = "Test chuyen tien",
                format = "text",
                template = "print"
            };
            var response = await bankService.GetQRCode(bankRequest);
            // lấy ra qrDataURL trong response.data
            var image = bankService.ConvertBase64ToImage(response.data.qrDataURL);

            return File(image, "image/png");
        }

        [HttpPost("Test-DateOnly")]// yyyy-MM-dd
        public async Task<IActionResult> GetBanks([FromForm] DateOnly dateOnly)
        {
            var date = dateOnly;
            return Ok();
        }
    }
}

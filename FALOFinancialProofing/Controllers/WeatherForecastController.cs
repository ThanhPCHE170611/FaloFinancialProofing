using FALOFinancialProofing.Attributes;
using FALOFinancialProofing.Attributes.RoleAttributes;
using FALOFinancialProofing.Helpers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
    }
}

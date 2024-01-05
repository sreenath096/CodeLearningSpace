using Microsoft.AspNetCore.Mvc;

namespace LinkedInPost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        private readonly IConfiguration _configuration;

        public WeatherForecastController(IConfiguration configuration)
        {            
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // indexer
            int weatherForecastMinRange = Convert.ToInt32(_configuration["weatherForecastMinRange"]);
            int weatherForecastMaxRange = Convert.ToInt32(_configuration["weatherForecastMaxRange"]);

            // using GetValue method without passing default value
            weatherForecastMinRange = _configuration.GetValue<int>("weatherForecastMinRange");
            weatherForecastMaxRange = _configuration.GetValue<int>("weatherForecastMaxRange");

            // using GetValue method by passing default value
            weatherForecastMinRange = _configuration.GetValue<int>("weatherForecastMinRange", 1);
            weatherForecastMaxRange = _configuration.GetValue<int>("weatherForecastMaxRange", 5);

            return Enumerable.Range(weatherForecastMinRange, weatherForecastMaxRange).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

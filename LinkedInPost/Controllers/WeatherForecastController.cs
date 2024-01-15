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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get([FromServices] IConfiguration configuration)
        {
            // indexer
            int weatherForecastMinRange = Convert.ToInt32(configuration["weatherForecastMinRange"]);
            int weatherForecastMaxRange = Convert.ToInt32(configuration["weatherForecastMaxRange"]);

            // using GetValue method without passing default value
            weatherForecastMinRange = configuration.GetValue<int>("weatherForecastMinRange");
            weatherForecastMaxRange = configuration.GetValue<int>("weatherForecastMaxRange");

            // using GetValue method by passing default value
            weatherForecastMinRange = configuration.GetValue<int>("weatherForecastMinRange", 1);
            weatherForecastMaxRange = configuration.GetValue<int>("weatherForecastMaxRange", 5);

            return Enumerable.Range(weatherForecastMinRange, weatherForecastMaxRange)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}

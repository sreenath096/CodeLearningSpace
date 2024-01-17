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
            // using indexer
            int weatherForecastMinRange = Convert.ToInt32(configuration["WeatherForecast:MinRange"]);
            int weatherForecastMaxRange = Convert.ToInt32(configuration["WeatherForecast:MaxRange"]);

            // using GetSection method
            weatherForecastMinRange = Convert.ToInt32(configuration.GetSection("WeatherForecast:MinRange").Value);
            weatherForecastMaxRange = Convert.ToInt32(configuration.GetSection("WeatherForecast:MaxRange").Value);

            // using GetSection and GetValue method without passing default value
            weatherForecastMinRange = configuration.GetSection("WeatherForecast").GetValue<int>("MinRange");
            weatherForecastMaxRange = configuration.GetSection("WeatherForecast").GetValue<int>("MaxRange");

            // using GetSection and GetValue method passing default value
            weatherForecastMinRange = configuration.GetSection("WeatherForecast").GetValue<int>("MinRange", 1);
            weatherForecastMaxRange = configuration.GetSection("WeatherForecast").GetValue<int>("MaxRange", 2);

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

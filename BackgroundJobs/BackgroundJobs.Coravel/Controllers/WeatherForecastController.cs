using BackgroundJobs.Coravel.Invocables;
using Coravel.Scheduling.Schedule.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobs.Coravel.Controllers
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(IScheduler scheduler)
        {
            var weatherData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 35),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
          .ToArray();

            scheduler.ScheduleWithParams<WeatherForecastJob>($"{weatherData.First().Date}: {weatherData.First().Summary}").EveryFiveSeconds();

            return weatherData;
        }
    }
}

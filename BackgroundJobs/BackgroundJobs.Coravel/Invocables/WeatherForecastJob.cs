using Coravel.Invocable;

namespace BackgroundJobs.Coravel.Invocables
{
    public class WeatherForecastJob : IInvocable
    {
        private readonly ILogger<WeatherForecastJob> _logger;
        private readonly string _temperatureReport;

        public WeatherForecastJob(ILogger<WeatherForecastJob> logger, string temperatureReport)
        {
            this._logger = logger;
            this._temperatureReport = temperatureReport;
        }

        public async Task Invoke()
        {
            _logger.LogInformation(_temperatureReport);
        }
    }
}


using System.ComponentModel;

namespace BackgroundJobs.HostedService
{
    public class RepeatingWorkerService : BackgroundService
    {
        private ILogger<RepeatingWorkerService> _logger;
        //private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(2000));

        public RepeatingWorkerService(ILogger<RepeatingWorkerService> Logger)
        {
            _logger = Logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"RepeatingWorkerService running cycle at: {DateTime.Now.ToString("O")}");
                await Task.Delay(3000, stoppingToken);
            }
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (await _timer.WaitForNextTickAsync(stoppingToken) &&
        //           !stoppingToken.IsCancellationRequested)
        //    {
        //        await DoWork();
        //    }
        //}

        //private async Task DoWork()
        //{
        //    _logger.LogInformation($"RepeatingWorkerService running cycle at: {DateTime.Now.ToString("O")}");
        //}
    }
}

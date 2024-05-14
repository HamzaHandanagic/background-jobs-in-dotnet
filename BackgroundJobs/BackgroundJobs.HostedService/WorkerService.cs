
namespace BackgroundJobs.HostedService
{
    internal class WorkerService : IHostedService
    {
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WorkerService running at: {time}", DateTimeOffset.Now.ToString("O"));
            await Task.Delay(10000, cancellationToken);
        }

        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _ = Task.Run(async () =>
        //    {
        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            _logger.LogInformation("WorkerService running at: {time}", DateTimeOffset.Now.ToString("O"));
        //            await Task.Delay(4000, cancellationToken);
        //        }
        //    }, cancellationToken);

        //    return Task.CompletedTask;
        //}

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker stopped");
        }
    }
}

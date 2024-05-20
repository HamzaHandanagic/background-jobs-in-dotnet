

using Microsoft.Extensions.DependencyInjection;

namespace BackgroundJobs.HostedLifecycleService
{
    public class WorkerServiceB : BackgroundService
    {
        private readonly ILogger<WorkerServiceB> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WorkerServiceB(ILogger<WorkerServiceB> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"StartAsync started of WorkerServiceB!");

            await SyncUsers();
        }

        private async Task SyncUsers()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            await using ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var users = dbContext.Users.Where(u => u.Role == "admin");
            foreach (var user in users)
            {
                _logger.LogInformation($"Sync admin user {user.FirstName}: {DateTime.Now.ToString("O")}");
            }
        }
    }
}

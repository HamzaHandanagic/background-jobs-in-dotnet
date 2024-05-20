using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs.HostedLifecycleService
{
    public class WorkerServiceA : IHostedLifecycleService
    {
        private readonly ILogger<WorkerServiceA> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WorkerServiceA(IServiceProvider serviceProvider, ILogger<WorkerServiceA> logger)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken) 
        {
            _logger.LogInformation($"StartAsync started of WorkerServiceA!");
        }
    
        public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"StartingAsync started of WorkerServiceA!");

            using IServiceScope scope = _serviceProvider.CreateScope();

            await using ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure that the database is created and apply any pending migrations.
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync(cancellationToken);

            var users = new List<User>()
            {
                new User { Id = 1, FirstName = "Admin1", LastName = "Admin1", Email = "admin1@test.com", Role = "admin" },
                new User { Id = 2, FirstName = "Admin2", LastName = "Admin2", Email = "admin2@test.com" ,Role = "admin" },
                new User { Id = 3, FirstName = "User1", LastName = "User1", Email = "user1@test.com", Role = "user" },
            };

            foreach (var user in users)
            {
                dbContext.Users.AddIfNotExists(user, p => p.Id == user.Id &&
                                                          p.FirstName == user.FirstName);
            }
         
            await dbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
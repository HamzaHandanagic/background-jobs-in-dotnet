using Coravel.Invocable;

namespace BackgroundJobs.Coravel.Invocables
{
    public class RecurringJob : IInvocable
    {
        private readonly ILogger<RecurringJob> _logger;

        public RecurringJob(ILogger<RecurringJob> logger)
        {
            _logger = logger;
        }

        public Task Invoke()
        {
            _logger.LogInformation($"RecurringJob at {DateTimeOffset.UtcNow}!");
            return Task.CompletedTask;
        }
    }
}

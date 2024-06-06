using Hangfire;
using System;

namespace BackgroundJobs.Hangfire.Jobs
{
    // Static class implementation
    // Basic use cases:
    public static class HangfireJobs
    {
        public static void ScheduleJobs(this IApplicationBuilder app)
        {
            // Fire-and-forget job
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget job executed"));

            // Delayed job
            BackgroundJob.Schedule("delayed-job", () => Console.WriteLine("Delayed job executed (15s)."), TimeSpan.FromSeconds(15));

            // Recurring job
            RecurringJob.AddOrUpdate("recurring-job", () => Console.WriteLine("Recurring job executed (each minute)"), Cron.Minutely());

            // Recurring job - custom cron expression
            RecurringJob.AddOrUpdate("custom-recurring-job", () => RecurringJobMethod("param1", 4), "0 9 * * 1");

            // Continuation job
            var parentJobId = BackgroundJob.Enqueue("parent-job-1", () => Console.WriteLine("Parent job executed"));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("Continuation job executed"));
        }

        public static void RecurringJobMethod(string param1, int param2)
        {
            Console.WriteLine($"Executing weekly job. Each Monday at 09:40AM. : {param1}, {param2}");
        }
    }
}
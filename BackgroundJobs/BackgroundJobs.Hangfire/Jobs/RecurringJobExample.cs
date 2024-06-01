using Hangfire;
using System;

namespace BackgroundJobs.Hangfire.Jobs
{
    public static class RecurringJobExample
    {
        public static void StartRecurringJobs(this IApplicationBuilder app) 
        {
            MinutelyRecurringJob(app);
            CustomWeeklyRecurringJob(app);
        }

        public static void MinutelyRecurringJob(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate("easyRecurringJob", () => Console.WriteLine("Recurring job: Each minute!"), Cron.Minutely);
        }

        // Every monday at 09:40 AM - cron expression
        // Identifiers should be unique
        public static void CustomWeeklyRecurringJob(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate(Guid.NewGuid().ToString(), () => Console.WriteLine("Executing weekly job. Each Monday at 09:40AM."), "40 9 * * 1");
        }

        
    }
}


using Quartz;

namespace BackgroundJobs.Quartz.Jobs
{
    public class BasicBackgroundJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var jobDataMap = context.MergedJobDataMap;
            await Console.Out.WriteLineAsync("Executing job.");
        }
    }
}

using BackgroundJobs.Quartz.Jobs;
using Microsoft.Extensions.Hosting;
using Quartz;

internal class Program
{
    //private static async Task Main(string[] args)
    //{
    //    var builder = Host.CreateDefaultBuilder()
    //    .ConfigureServices((cxt, services) =>
    //    {
    //        services.AddQuartz();
    //        services.AddQuartzHostedService(opt =>
    //        {
    //            opt.WaitForJobsToComplete = true;
    //        });
    //    }).Build();

    //    var job = JobBuilder.Create<BasicBackgroundJob>()
    //        .WithIdentity(name: "BackgroundJob", group: "JobGroup")
    //        .Build();

    //    var trigger = TriggerBuilder.Create()
    //        .WithIdentity(name: "SimpleRepeatingTrigger", group: "TriggerGroup")
    //        .WithSimpleSchedule(o => o
    //            .RepeatForever()
    //            .WithIntervalInSeconds(5))
    //        .Build();

    //    var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
    //    var scheduler = await schedulerFactory.GetScheduler();
    //    await scheduler.ScheduleJob(job, trigger);

    //    await builder.RunAsync();
    //}


    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(BasicBackgroundJob));

            configure
                .AddJob<BasicBackgroundJob>(jobKey)
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        builder.Services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        var app = builder.Build();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
#### BackgroundJobs.Quartz.NET

There are three important concepts in Quartz.NET:

- Job - the actual background task you want to run
- Trigger - the trigger controlling when a job runs
- Scheduler - responsible for coordinating jobs and triggers


We need to add packages: NuGet packages:  Quartz, Quartz.Extensions.Hosting and Microsoft.Extensions.Hosting packages. 

Whenever we want to define a job in Quartz.NET, we need to implement the IJob interface. This interface contains one method which is triggered whenever job is executed:

```
Task Execute(IJobExecutionContext context);
```

 The IJobExecutionContext parameter contains information about the environment, such as the details of the job and the trigger that executed the job.

 We need to define an IJobDetail object that is tied to our job class:

 ```
IJobDetail job = JobBuilder.Create<MyJob>()
    .WithIdentity(name: "MyJob", group: "JobGroup")
    .Build();
```

To trigger job, we use the TriggerBuilder class to define and create an ITrigger object. That trigger object needs to be added to the scheduler. 

Example of a trigger:

```
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity(name: "SimpleTrigger", group: "TriggerGroup")
    .WithSimpleSchedule(s => s
        .WithRepeatCount(10)
        .WithInterval(TimeSpan.FromSeconds(10)))
    .Build();
```

The scheduler manages the lifecycle of jobs and triggers and is responsible for scheduling related operations, such as pausing triggers, etc. We gain access to an IScheduler instance by retrieving it from the SchedulerFactory:

```
var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();
await scheduler.ScheduleJob(job, trigger);
```

It is possible to pass data to jobs:

```
 .UsingJobData("ConsoleOutput", "Executing background job using JobDataMap")
 ```

 And then consume it:
 ```
 var jobDataMap = context.MergedJobDataMap;
  var useJobDataMapConsoleOutput = jobDataMap.GetBoolean("UseJobDataMapConsoleOutput");
  ```

 It is possible to use persistence store:

```
  services.AddQuartz(opt =>
{
    opt.UsePersistentStore(s =>
    {
        s.UseSqlServer("<CONNECTION_STRING>");
        s.UseJsonSerializer();
    });
});
```

NOTES:
- Setting the WaitForJobsToComplete option to true will ensure that Quartz.NET waits for the jobs to complete gracefully before exiting.
- By default, Quartz configures all jobs using the RAMJobStore which is the most performant because it keeps all of its data in RAM. However, this also means it's volatile and you can lose all scheduling information when your application stops or crashes. It can be configured to use SQL data store.
- Quartz.NET will create jobs by fetching them from the DI container. This means you can use scoped services in your jobs, not just singleton or transient services which is not the case in IHostedService
- We need to uniquely identify our background job with a JobKey
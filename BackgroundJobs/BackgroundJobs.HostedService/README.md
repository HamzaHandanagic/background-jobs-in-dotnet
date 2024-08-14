### Background Jobs in .NET - IHostedService

#### Differences between IHostedService implementation and BackgroundService implementation

IHostedService interface offers two methods for handling background tasks:

*StartAsync* - starts a background task. No other services can start until the StartAsync method concludes, which is why using IHostedService is not advisable for long-running or concurrent tasks. If an action takes time then this is an issue.

*StopAsync* - ends the background task and is triggered when the application host performs a graceful shutdown. However, if an error or unexpected failure occurs in an application, StopAsync may not be called.

Because of StartAsync method, in .NET Core 3.0 BackgroundService was added as a helper class. BackgroundService is encapsulating all necessary steps within its single ExecuteAsync method. This eliminates the need for you to define StartAsync, StopAsync, and other parameters.

*IHostedService* – typically used for short-running tasks

*BackgroundService* – typically used for long-running tasks

#### Worker class

Issue is that the whole process must finish before application can continue.  StartAsync never returns and taht is an issue. Awaiting process must finish before we move on into running the application.

One workaround was with Task.Run (kind of a fire and forget) but that is also not great practice because we don't have any idea about exception, has task finished etc. Task is scheduled on the thread pool.

Before .NET 8  host starts and stops hosted services sequentially. Each service StartAsync is required to complete before the next one can start. Issues here: slow application warm-up, scaling issue etc. When stopping applications, the same sequential behaviour occurred, this time with services being stopped in reverse order.  This behaviour can be more problematic at shutdown as there is a timeout configured that limits how long the graceful shutdown can take.

In .NET 8 there is a solution for this with two new options which allow us to switch to a concurrent start and/or stop behaviour. To achieve this, we can configure the HostOptions, setting start and stop behaviour to run concurrently. Behind the scenes, in the Host implementation Task.Run is being used. Tasks are triggered in the order they are registered but they are not awaited. In this mode, the startup will take only as long as the slowest StartAsync call, allowing the application as a whole to start more quickly.

Don't use it in situations where your hosted services depend directly on one another in some way. Another example of when you don't want to use these concurrent hosted service: db migration. You want to first migrate data and then your application to start listening to requests. 


#### RepeatingWorkerService class

2 types of services: 
1.	On demand: processes that need to occur now. Examples: emails at some action, payment processing, webhooks etc.
	
2.	Recurring or Scheduled Tasks: hourly, daily, weekly. Examples: resource cleanups: logs, reports, refreshing cache etc.

In this case RepeatingWorkerService is going to run each 2 seconds and do some repeatable work. Task.Delay approach here is not optimal. There are  timers in .NET like System.Threading.Timer which can be used for this. This timer works by executing a single callback on a thread pool at regular intervals. These timers are tricky: they are using callbacks, they can cause memory leaks and there is a small loss of a few miliseconds. Accuracy is an issue.

.NET 6 introduced new .NET Timer called **PeriodicTimer** which should be used in these cases of repeatable cases. This timer will still have a few milliseconds difference from a round second, but in each repeat PeriodicTimer will try to compensate for the difference in time.  Also, with PeriodicTimer we can cancel a timer before it completes its intended executions, i.e., when conditions change and we need to halt the operation. 

*NOTE*: This timer is intended to be used only by a single consumer at a time: only one call to WaitForNextTickAsync(CancellationToken) may be in flight at any given moment




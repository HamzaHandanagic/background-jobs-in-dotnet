#### BackgroundJobs.Hangfire

Hangfire offers a straightforward solution for executing background tasks and processes in a .NET web application, with support for persistent storage of both jobs and queues. Library consists of three main components:
- Hangfire Client - application code that enqueues jobs and schedules them to run. Different ways of background jobs.
- Hangfire Server - background process that runs continously and is responsible for executing jobs. Implemented as a set of dedicated background threads. Server is also keeping the storage clean and removing old data automatically.
- Job Storage - database to store all the information related to background jobs such as: definitions, execution status etc. By default it uses SqlServer as a persistance storage. It helps on application restarts, server reboots etc.

Workflow: Client creates the job, job is being stored in job repository and returned to client. Hangfire server is fetching jobs from the repository and executing them.


It accommodates fire-and-forget jobs as well as "cron"-based scheduling. Additionally, Hangfire includes an intuitive dashboard where you can monitor completed and failed jobs, and provides the ability to retry failed jobs as needed.


Hangfire in free version offers:

- Fire-and-Forget Jobs - Fire-and-forget jobs are executed only once and almost immediately after creation.

- Delayed jobs - Delayed jobs are executed only once too, but not immediately, after a certain time interval.

- Recurring Jobs - Recurring jobs fire many times on the specified CRON schedule.

- Continuations jobs - are executed when its parent job has been finished.

In Pro version there are:
- Batches - Batch is a group of background jobs that is created atomically and considered as a single entity.

- Batch continuation - is fired when all background jobs in a parent batch finished.

**Setup:**

The ASP.NET Core application uses two Hangfire Nuget packages to integrate the event handling into the solution.

- Hangfire.AspNetCore
- Hangfire.SqlServer

Code setup:

- Hangfire will create tables but you must provide a database. Provide the connection string.
- Add Hangfire to the Services collection and add Hangfire server with the *AddHangfireServer()* method.
- Big advantage of hangfire is built-in dashboard which we can add with *UseHangfireDashboard()*. With dashboard we can see status of jobs, cancel running jobs etc. 
- Setup in client: does not require to create special classes. Background jobs are based on regular static or instance methods invocation.
```
var client = new BackgroundJobClient();

client.Enqueue(() => Console.WriteLine("Easy!"));
client.Delay(() => Console.WriteLine("Reliable!"), TimeSpan.FromDays(1));
```
We can also use the *BackgroundJob* class that allows you to use static methods to perform the creation task:
```
BackgroundJob.Enqueue(() => Console.WriteLine("Hello!"));
```


**Benefits:**

- Popular library.
- Built-in dashboard. Auhorization can be added.
- Complex scheduling needs and use cases. 

**Cons:**
- You need database: e.g.: MySQL or PostgreSQL.
- 

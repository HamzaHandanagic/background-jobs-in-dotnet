
### BackgroundJobs.HostedLifecycleService

 In .NET 8, focus has been put on introducing more control over the startup behaviour of hosted services. In .NET 8 we have an option to concurrent start and/or stop services but also there is new interface in the Microsoft.Extensions.Hosting namespace called IHostedLifecycleService.

This interface can help us extend background tasks to add methods for new lifecycle events which occur before or after the existing StartAsync or StopAsync methods. Interface is defined as:
~~~
public partial interface IHostedLifecycleService : Microsoft.Extensions.Hosting.IHostedService
{
    Task StartingAsync(CancellationToken cancellationToken);
    Task StartedAsync(CancellationToken cancellationToken);
    Task StoppingAsync(CancellationToken cancellationToken);
    Task StoppedAsync(CancellationToken cancellationToken);    
}
~~~

This is useful for more advanced scenarios and provide a way to be more specific about some actions.

The `StartingAsync` method for all registered hosted services that implement this interface will run very early in the application lifecycle, before `StartAsync` (from `IHostedService`) is called on any registered hosted services. This can be used to perform early validation checks before the application starts, such as verifying the availability of critical requirements or dependencies. This enables the application to fail startup if necessary, before any hosted services begin executing their primary workload. Other uses include pre-heating and initializing singletons and other state that the application requires.

The `StartedAsync` method will be invoked after all `StartAsync` (from `IHostedService`) methods for registered hosted services have completed. This can be used to validate the application state or conditions just before marking the application as successfully started.


#### Example

One common scenario involves initializing databases. Although alternative solutions exist, this task can also be accomplished using hosted services. Typically, the aim is to carry out database initialization at the beginning of the application lifecycle, before the primary workloads start.

Let's assume we have WorkerServiceA (which initializes the database) and WorkerServiceB (which uses the initialized data to sync with Azure AD users).


In .NET 7 default behaviour is starting each hosted service StartAsync method in sequence rather than concurrently and in this case WorkerServiceB will be started after StartAsync of WorkerServiceA is finished which is adequate for our solution. Using new options in .NET 8 with IHostedLifecylceService we could reduce the overall startup time of the application. We can utilize concurrent execution and also new methods in IHostedLifecylceService like StartingAsync to execute workload like database seed before other work.


So, we can put our database setup code in the StartingAsync method. ServiceB, which is a BackgroundService, can then safely use the database in its ExecuteAsync method. This method is called by StartAsync, defined in the IHostedService interface, only after all StartingAsync methods for registered services have completed.

Example explanation:

- Simple setup with .NET Core Web API, EF Core, SQLite. Use case: internal application with users. We need to provide seeded users to access our app and also sync them with our Azure AD. Database with one table User. On application start users need to be seeded and then those users synced to Azure AD.
- WorkerServiceA - first hosted service that is used to initialize database.
- WorkerServiceB - seecond hosted service that is going to use initialized data to do some work. Let's assume sync with Azure AD.

- Packages used: *Microsoft.EntityFrameworkCore.Sqlite*, *Microsoft.EntityFrameworkCore.Design*
- Manually way to do migrations and db:
        - *dotnet ef migrations add init*
        - *dotnet ef database update*

**Notes**: 


- You can only inject transient or singleton services.
- To use scoped services within a BackgroundService, create a scope. No scope is created for a hosted service by default.
- The EF DbContext is a scoped service, which we can't inject directly inside of WorkerServiceA. We have to inject an instance of IServiceProvider which we can use to create a custom service scope, so that we can resolve the scoped AppDbContext.
- In this example migrations are done in hosted service. This is not recommended.
- Don't use concurrent options with .NET 8 if one hosted service is dependant on the other service

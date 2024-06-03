### CORAVEL

Name is inspired from .NET Core and Laravel (author found inspiration in background processing system in Laravel).

Used for: task scheduling, caching, queueing, mailing, event broadcasting. Main purpose is task scheduling. There is a Pro version with additional features (dashboard).

Designed for .NET Core apps. Very simple. Natural. API created for .NET. It uses Fluent API. In memory scheduling. No persistence.

Simple setup:

1. Add Coravel package:

~~~
dotnet add package coravel
~~~

2. Add UseScheduler() to services.

~~~
builder.Services.AddScheduler();
~~~

3. Use Scheduler

~~~
app.Services.UseScheduler(scheduler =>
{
    scheduler.ScheduleAsync(async () =>
    {
        Console.WriteLine("Do Work!");
        await Task.Delay(2000);
    }).EverySeconds(2);
});
~~~

This can be encapsulated into a class:

~~~
app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<ReccuringJob>().EveryFiveSeconds();
});
~~~

And registered within DI container:

~~~
builder.Services.AddTransient<RecurringJob>();
~~~



Important concept is concept of Invocable. Invocables represent a self-contained job within your system. This is a class that implements IInvocable interface. We need to implement the Invoke method when implementing interface. Using .NET Core's dependency injection services, your invocables will have all their dependencies injected whenever they are executed.

There are other options for configuration: 

Schedule With parameters that can be consumed from Dependency Injection:

~~~
app.Services.UseScheduler(scheduler =>
{
    scheduler.ScheduleWithParams<RecurringJob>("some_paramater").EveryFiveSeconds();
});
~~~

Prevent overlap with another task which can prevent some issues:

~~~
app.Services.UseScheduler(scheduler =>
{
    scheduler.ScheduleWithParams<RecurringJob>("some_paramater")
             .EveryFiveSeconds()
             .PreventOverlapping("id");
});
~~~

It can be easily integrated as a part of your application flow, E.g.: when user is registered you want to send him welcome email and this can be easily configured into application workflow.


Benefits:
- Multiple use cases for web apps
- Lightweight compared to more popular Hangfire or Quartz
- Simplistic syntax, really easy to configure
- Supports async/await out of the box
- Integration with Dependency Injection
- Highly configurable: prevent overlapping, different scheduling mechanism etc.


Compared to Hangfire:

- Hangfire still does not natively support true async/await. 

Compared to Quartz:

- Quartz is not really compatible with modern .NET. It can be used but there are some things like dependency injection support that are lacking.

using BackgroundJobs.Coravel.Invocables;
using Coravel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.

builder.Services.AddScheduler();
builder.Services.AddTransient<RecurringJob>();

var app = builder.Build();

//app.Services.UseScheduler(scheduler =>
//{
//    scheduler.ScheduleAsync(async () =>
//    {
//        Console.WriteLine("Do Work!");
//        await Task.Delay(2000);
//    }).EverySeconds(2);
//});

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<RecurringJob>()
             .EveryFiveSeconds();
});

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();
app.Run();

using BackgroundJobs.Hangfire.Jobs;
using Hangfire;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Configure the HTTP request pipeline.
        var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

        // Add and configure Hangfire services
        builder.Services.AddHangfire(configuration =>
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseSqlServerStorage(connectionString));

        builder.Services.AddHangfireServer();

        var app = builder.Build();

        app.UseHangfireDashboard();

        // Extract reccuring jobs to separate static class
        app.ScheduleJobs();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
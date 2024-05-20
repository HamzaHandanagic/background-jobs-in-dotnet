using BackgroundJobs.HostedLifecycleService;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlite("Data Source=hostedlifecylceexample.db"));

        builder.Services.AddHostedService<WorkerServiceA>();
        builder.Services.AddHostedService<WorkerServiceB>();

        builder.Services.Configure<HostOptions>(x =>
        {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = true;
        });

        builder.Services.AddControllers();

        var app = builder.Build();

        app.Run();
    }
}
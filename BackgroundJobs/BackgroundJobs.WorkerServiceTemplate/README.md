### Background Jobs in .NET - Worker Service template

For standalone background service there is a Worker template. It will add most basic implementation of Worker that is using BackgroundService. 

At Program.cs we have Host. Host from Microsoft.Extension.Hosting package is helping us to create long running services. Host concept is also relevant in other ASP.NET Core apps, not just hosted services.  It starts inside the Kestrel web server which start listening on certain ports and then request is going through the pipeline. The host is responsible for app startup and lifetime management. At a minimum, the host configures a server and a request processing pipeline. The host can also set up logging, dependency injection, and configuration.

The service must be registered within dependency injection container in Program.cs. We use extension method named AddHostedService on IServiceCollection. This generic method accepts the type that represents the worker service. This will add IHostedService registration for a given type. It will add it as a Singleton.

If there is a need for horizontal scaling then extracting background logic to separate project is good idea. Also, if our background job doesn't have too much dependecies on other services.

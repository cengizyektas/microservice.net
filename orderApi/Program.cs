using basketApi.Context;
using orderApi;
using orderApi.Helpers;
using orderApi.Interface;
using RabbitMQ.Client;

public class Program
{
    public static void Main(string[] args)
    {

        IHostBuilder hostBuilder = CreateHostBuilder(args);
        hostBuilder.Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddHealthChecks();
                });
                webBuilder.UseStartup<Startup>();
            });
}




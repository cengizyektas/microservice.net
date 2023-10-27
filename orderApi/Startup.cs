
using basketApi.Context;
using Consul;
using orderApi.Helpers;
using orderApi.Interface;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IServiceCallHelper, ServiceCallHelpers>();
        services.AddDbContext<OrderDbContext>();
        services.AddControllers();
        services.AddSwaggerGen();

        //services.AddCap(option =>
        //{
        //    option.UseEntityFramework<OrderDbContext>();
        //    option.UseDashboard(x => x.PathMatch = "/cap");
        //    string db_setting = String.Format("Host={0}; Port={1}; Username={2}; Password={3}; Database={4};", "192.168.0.14", 5432, "root", "root", "Emarket");

        //    option.UsePostgreSql(db_setting);
        //    option.UseRabbitMQ(option =>
        //    {
        //        option.ConnectionFactoryOptions = option =>
        //        {
        //            option.Ssl.Enabled = false;
        //            option.HostName = "192.168.0.24";
        //            option.Port = 5672;
        //            option.UserName = "guest";
        //            option.Password = "guest";
        //        };

        //    });
        //});


        services.AddEndpointsApiExplorer();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IHostApplicationLifetime lifetime)
    {
        ServiceEntity serviceEntity = new ServiceEntity
        {
            IP = "localhost",
            Port = 5001,
            ServiceName = "customer-api",
            ConsulIP = "localhost",
            ConsulPort = 8500
        };
        app.RegisterConsul(lifetime, serviceEntity);

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });


    }
}

public static class AppBuilderExtensions
{
    public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ServiceEntity serviceEntity)
    {
        var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulIP}:{serviceEntity.ConsulPort}"));
        var httpCheck = new AgentServiceCheck()
        {
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
            Interval = TimeSpan.FromSeconds(10),
            HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}/healt",
            Timeout = TimeSpan.FromSeconds(5)
        };
        var registration = new AgentServiceRegistration()
        {
            Checks = new[] { httpCheck },
            ID = Guid.NewGuid().ToString(),
            Name = serviceEntity.ServiceName,
            Address = serviceEntity.IP,
            Port = serviceEntity.Port,
            Tags = new[] { $"urlprefix-/{serviceEntity.ServiceName}" }
        };
        consulClient.Agent.ServiceRegister(registration).Wait();
        lifetime.ApplicationStopping.Register(() =>
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
        return app;
    }
}

public class ServiceEntity
{
    public string IP { get; set; }
    public int Port { get; set; }
    public string ServiceName { get; set; }
    public string ConsulIP { get; set; }
    public int ConsulPort { get; set; }
}
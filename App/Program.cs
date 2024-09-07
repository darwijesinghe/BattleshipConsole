using App;
using Services.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Services.Classes;
using Services.Services.Interfaces;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        // add services
        var host = Host.CreateDefaultBuilder()
             .ConfigureAppConfiguration((context, config) =>
             {
                 config.SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
             })
            .ConfigureServices((context, services) =>
            {
                // Register logging service
                services.AddLogging();

                // Register configurations
                services.AddSingleton(context.Configuration);

                // Bind configuration settings
                services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));

                // Register a single HttpClient instance
                services.AddHttpClient("GlobalClient", client =>
                {
                    var apiSettings    = context.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                    client.BaseAddress = new Uri(apiSettings.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

                // Register services
                services.AddTransient<IShipService, ShipService>();
                services.AddTransient<IShootService, ShootService>();

            })
            .Build();

        // automatically resolve and inject any dependencies required by the Startup class's constructor.
        // this helps in creating instances of classes that have dependencies registered in the DI container

        var service = ActivatorUtilities.CreateInstance<Startup>(host.Services);
        await service.Run();
    }
}
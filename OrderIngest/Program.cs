namespace OrderIngest
{
    using Microsoft.Extensions.AI;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OrderIngest.Services;
    using OrderIngest.Services.Common;
    using OrderIngest.Services.HttpClients;
    using OrderIngest.Services.OrderTranslation;

    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            // Load configuration from JSON files
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                 .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                                 .AddUserSecrets<Program>();

            // Register loggers
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IOrderLogger, OrderLogger>();

            // Register UI
            //builder.Services.AddHostedService<UserInterfaceBackgroundService>();
            builder.Services.AddSingleton<UserInterface>();

            // Register services
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IAlertApiClient, AlertApiClient>();
            builder.Services.AddSingleton<IOrderTranslator, OrderTranslator>();
            builder.Services.AddSingleton<IOrderProcessor, OrderProcessor>();
            builder.Services.AddSingleton<IOpenAIClientsWrapper, OpenAIClientsWrapper>();

            IHost host = builder.Build();

            host.Services.GetRequiredService<UserInterface>().Launch();
        }
    }
}

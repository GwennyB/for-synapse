namespace OrderIngest
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            // Load configuration from JSON files
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                 .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                                 .AddUserSecrets<Program>();

            IHost host = builder.Build();
        }
    }
}

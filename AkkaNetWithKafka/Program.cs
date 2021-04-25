using AkkaNetWithKafka.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AkkaNetWithKafka
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                // register our host service
                services.AddHostedService<KafkaService>();

                })
                .ConfigureLogging((hostContext, configLogging) =>
                {                    
                    configLogging.AddConsole();

                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}

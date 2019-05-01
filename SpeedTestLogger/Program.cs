using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SpeedTestLogger.Host.Generic;

namespace SpeedTestLogger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            GenericHost
                .CreateDefaultBuilder(args)
                .UseStartup(config => new Startup(config));
    }
}

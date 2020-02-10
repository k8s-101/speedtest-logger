using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

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
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var startup = new Startup(context.Configuration);
                    startup.ConfigureServices(services, context.HostingEnvironment);
                });
    }
}

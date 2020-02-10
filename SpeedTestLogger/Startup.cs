using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeedTest;

namespace SpeedTestLogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services, IHostEnvironment environment)
        {
            var singleRun = bool.Parse(Configuration["singleRun"]);
            var userId = Configuration["userId"];
            var loggerId = Int32.Parse(Configuration["loggerId"]);
            var uploadResults = bool.Parse(Configuration["uploadResults"]);
            services.AddTransient<LoggerHostConfiguration>(provider => new LoggerHostConfiguration
            {
                SingleRun = singleRun,
                UserId = userId,
                LoggerId = loggerId,
                UploadResults = uploadResults,
            });

            var apiUrl = new Uri(Configuration["speedTestApiUrl"]);
            services.AddTransient<SpeedTestApiClient>(provider => new SpeedTestApiClient(apiUrl));

            var kubeMQAddress = Configuration["KubeMQ_ServerAddress"];
            var kubeMQChannel = Configuration["KubeMQ_Channel"];
            services.AddTransient<KubeMQClient>(provider => new KubeMQClient(kubeMQAddress, loggerId, kubeMQChannel));

            services.AddTransient<SpeedTestRunner>(provider => new SpeedTestRunner());

            // var loggerLocation = new RegionInfo(Configuration["loggerLocationCountryCode"]);
            // services.AddTransient<SpeedTestRunner>(provider =>
            //     new SpeedTestRunner(
            //         new SpeedTestClient(),
            //         loggerLocation));

            services.AddHostedService<LoggerHost>();
        }
    }
}

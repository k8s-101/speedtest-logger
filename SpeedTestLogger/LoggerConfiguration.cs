using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SpeedTestLogger
{
    public class LoggerConfiguration
    {
        public readonly string UserId;
        public readonly int LoggerId;
        public readonly RegionInfo LoggerLocation;
        public readonly bool UploadResults;
        public readonly Uri ApiUrl;

        public LoggerConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            UserId = configuration["userId"];
            LoggerId = Int32.Parse(configuration["loggerId"]);
            LoggerLocation = new RegionInfo(configuration["loggerLocationCountryCode"]);
            UploadResults = bool.Parse(configuration["uploadResults"]);
            ApiUrl = new Uri(configuration["speedTestApiUrl"]);
        }
    }
}

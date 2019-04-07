using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    class Program
    {
        private static LoggerConfiguration _config;
        private static SpeedTestRunner _runner;

        static async Task Main()
        {
            Console.WriteLine("Starting SpeedTestLogger");

            _config = new LoggerConfiguration();
            _runner = new SpeedTestRunner(_config.LoggerLocation);

            var testData = _runner.RunSpeedTest();
            Console.WriteLine("Got download: {0} Mbps and upload: {1} Mbps", testData.Speeds.Download, testData.Speeds.Upload);

            var results = new TestResult
            {
                SessionId = Guid.NewGuid(),
                User = _config.UserId,
                Device = _config.LoggerId,
                TestDate = DateTime.Now,
                Data = testData
            };

            if (!_config.UploadResults)
            {
                Console.WriteLine("Skipping data upload to speedtest API");
                return;
            }

            Console.WriteLine("Uploading data to speedtest API");
            var success = false;
            using (var client = new SpeedTestApiClient(_config.ApiUrl))
            {
                success = await client.PublishTestResult(results);
            }

            if (success)
            {
                Console.WriteLine("Speedtest complete!");
            }
            else {
                Console.WriteLine("Speedtest failed!");
            }
        }
    }
}

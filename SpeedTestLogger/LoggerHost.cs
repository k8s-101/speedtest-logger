using System;
using System.Threading;
using System.Threading.Tasks;
using KubeMQ.SDK.csharp.Events;
using Microsoft.Extensions.Hosting;
using SpeedTestLogger.Models;

namespace SpeedTestLogger
{
    public class LoggerHost : IHostedService
    {
        private readonly KubeMQClient _queue;
        private readonly SpeedTestRunner _runner;
        private readonly SpeedTestApiClient _apiClient;
        private readonly LoggerHostConfiguration _config;
        private readonly IHostApplicationLifetime _lifetime;

        public LoggerHost(KubeMQClient queue, SpeedTestRunner runner, SpeedTestApiClient apiClient, LoggerHostConfiguration config, IHostApplicationLifetime lifetime)
        {
            _queue = queue;
            _runner = runner;
            _apiClient = apiClient;
            _config = config;
            _lifetime = lifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting SpeedTestLogger");

            if (_config.SingleRun)
            {
                await RunSpeedTestAndUploadResults();
                _lifetime.StopApplication();
            }
            else {
                _queue.SubscribeToEvents(HandleIncomingEvents);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _apiClient.Dispose();

            Console.WriteLine("SpeedTestLogger stopped");

            await Task.CompletedTask;
        }

        private void HandleIncomingEvents(EventReceive @event)
        {
            RunSpeedTestAndUploadResults().GetAwaiter().GetResult();
        }

        private async Task RunSpeedTestAndUploadResults()
        {
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
            var success = await _apiClient.PublishTestResult(results);
            if (success)
            {
                Console.WriteLine("Speedtest complete!");
            }
            else
            {
                Console.WriteLine("Speedtest failed!");
            }
        }
    }
}
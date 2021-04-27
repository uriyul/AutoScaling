using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AutoScalingClient
{
    public class RandomRequestsSender
    {
        private readonly AutoScalingClient _autoScalingClient = new AutoScalingClient();
        private readonly Logger _logger = new Logger();
        private readonly int _minWait;
        private readonly int _maxWait;
        private readonly int _waitBetweenSends;
        private readonly int _maxTasks;

        public RandomRequestsSender()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            _minWait = int.Parse(config["MinWait"]);
            _maxWait = int.Parse(config["MaxWait"]);
            _waitBetweenSends = int.Parse(config["WaitBetweenSends"]);
            _maxTasks = int.Parse(config["MaxTasks"]);
        }

        public void SendRequestsBulk(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Task.Run(SendRandomRequest);
                Thread.Sleep(_waitBetweenSends);
            }
        }

        private async Task SendRandomRequest()
        {
            var random = new Random(DateTime.Now.Millisecond);
            int count = random.Next(_maxTasks);
            _logger.Log($"Adding {count} tasks");
            await _autoScalingClient.AddTasks(count);
            Thread.Sleep(random.Next(_minWait, _maxWait));
            await _autoScalingClient.ReportCompleted(count);
            _logger.Log($"Finished {count} tasks");
        }
    }


}

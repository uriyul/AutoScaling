using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoScalingClient
{
    public class RandomRequestsSender
    {
        private readonly AutoScalingClient _autoScalingClient = new AutoScalingClient();
        private readonly Logger _logger = new Logger();
        private const int _minWait = 30_000; // 30 seconds
        private const int _maxWait = 120_000; // 2 minutes
        private const int _waitBetweenSends = 5_000; // 5 seconds;
        private const int _maxTasks = 100_000;

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

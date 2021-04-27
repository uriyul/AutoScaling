using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoScalingServer.BL
{
    public class InstancesHandler : IInstancesHandler
    {
        private int _currentTasksCount;
        private readonly object _countLock = new object();
        private int _instanceCount;
        private readonly int _maxInstances;
        private readonly int _minInstances;
        private readonly int _tasksPerInstance;
        private readonly object _instanceLock = new object();
        private readonly ILogger _logger;

        public InstancesHandler(IConfiguration configuration, ILogger<InstancesHandler> logger)
        {
            _logger = logger;
            _instanceCount = int.Parse(configuration["InitialInstances"]);
            _maxInstances = int.Parse(configuration["MaxInstances"]);
            _minInstances = int.Parse(configuration["MinInstances"]);
            _tasksPerInstance = int.Parse(configuration["TasksPerInstance"]);
        }

        public void AddTasks(int tasksCount)
        {
            LoadBalance(tasksCount);
        }

        public void ReportEnded(int tasksCount)
        {
            LoadBalance(-tasksCount);
        }

        private void LoadBalance(int addedTasks)
        {
            lock (_countLock)
            {
                _currentTasksCount += addedTasks;
                if (_currentTasksCount < 0)
                {
                    throw new Exception("Negative number of tasks!");
                }
            }

            int desiredInstances = (_currentTasksCount - 1) / _tasksPerInstance + 1;

            lock(_instanceLock)
            {
                {
                    if (_instanceCount > desiredInstances)
                    {
                        RemoveInstances(_instanceCount - desiredInstances);
                    }
                    else if (_instanceCount < desiredInstances)
                    {
                        AddInstances(desiredInstances - _instanceCount);
                    }
                }
            }
        }

        private void AddInstances(int count)
        {
            if (_instanceCount + count > _maxInstances)
            {
                count = _maxInstances - _instanceCount;
                _instanceCount = _maxInstances;
            }
            else
            {
                _instanceCount += count;
            }
            
            _logger.LogInformation($"Adding {count} instances. Total instances: {_instanceCount}. Total tasks: {_currentTasksCount}.");
        }

        private void RemoveInstances(int count)
        {
            if (_instanceCount - count < _minInstances)
            {
                count = _instanceCount - _minInstances;
                _instanceCount = _minInstances;
            }
            else
            {
                _instanceCount -= count;
            }

            _logger.LogInformation($"Shutting down {count} instances. Total instances: {_instanceCount}. Total tasks: {_currentTasksCount}.");
        }
    }
}

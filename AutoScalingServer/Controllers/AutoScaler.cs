using Microsoft.AspNetCore.Mvc;
using AutoScalingServer.BL;
using Microsoft.Extensions.Logging;

namespace AutoScalingServer.Controllers
{
    [ApiController]
    [Route("api/v1/AutoScaler/")]
    public class AutoScaler : Controller
    {
        private IInstancesHandler _instancesHandler;
        private readonly ILogger _logger;

        public AutoScaler(IInstancesHandler instancesHandler, ILogger<AutoScaler> logger)
        {
            _instancesHandler = instancesHandler;
            _logger = logger;
        }

        /// <summary>
        /// Add translation tasks to the auto scaler
        /// </summary>
        /// <param name="taskCount">Number of requested tasks</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddTasks/")]
        public IActionResult AddTasks(int taskCount)
        {
            _logger.LogInformation($"Got request: AddTasks, count={taskCount}");
            _instancesHandler.AddTasks(taskCount);
            return Ok();
        }

        /// <summary>
        /// Report that tasks have ended
        /// </summary>
        /// <param name="taskCount">Number of ended tasks</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ReportEnded/")]
        public IActionResult ReportEnded(int taskCount)
        {
            _logger.LogInformation($"Got request: ReportEnded, count={taskCount}");
            _instancesHandler.ReportEnded(taskCount);
            return Ok();
        }
    }
}

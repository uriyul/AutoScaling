using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoScalingServer.BL
{
    public interface IInstancesHandler
    {
        void AddTasks(int tasksCount);
        void ReportEnded(int tasksCount);
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AutoScalingClient
{
    class AutoScalingClient
    {
        private readonly HttpClient _apiClient;
        private string _baseAddress = "https://localhost:44312/api/v1/AutoScaler/";
        private string _addTasks = "AddTasks?taskCount=";
        private string _ReportCompleted = "ReportEnded?taskCount=";

        public AutoScalingClient()
        {
            _apiClient = new HttpClient();
        }

        public async Task AddTasks(int count)
        {
            await _apiClient.GetAsync($"{_baseAddress}{_addTasks}{count}");
        }

        public async Task ReportCompleted(int count)
        {
            await _apiClient.GetAsync($"{_baseAddress}{_ReportCompleted}{count}");
        }
    }
}

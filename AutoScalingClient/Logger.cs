using System;
using System.Globalization;
using System.IO;

namespace AutoScalingClient
{
    public class Logger
    {
        private readonly string _filename;

        public Logger()
        {
            var path = Directory.GetCurrentDirectory();
            _filename = $@"{path}\..\..\..\..\Logs\Client.txt";
        }

        public void Log(string message)
        {
            lock (this)
            {
                File.AppendAllText(_filename, $"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")} {message}{Environment.NewLine}" );
            }
        }
    }
}

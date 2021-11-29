using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace TestSystem.API.Logging
{
    public class TextFileLogger : ILogger
    {
        private string filePath;
        private static object lockObj = new object();

        public TextFileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state)
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel == LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (lockObj)
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}

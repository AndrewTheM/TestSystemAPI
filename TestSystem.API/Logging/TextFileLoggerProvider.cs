using Microsoft.Extensions.Logging;

namespace TestSystem.API.Logging
{
    public class TextFileLoggerProvider : ILoggerProvider
    {
        private string filePath;

        public TextFileLoggerProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
            => new TextFileLogger(filePath);

        public void Dispose()
        {
        }
    }
}

using Microsoft.Extensions.Logging;
using TestSystem.API.Logging;

namespace TestSystem.API.Extensions
{
    public static class TextFileLoggerExtensions
    {
        public static ILoggingBuilder AddTextFile(this ILoggingBuilder builder, string filePath)
        {
            builder.AddProvider(new TextFileLoggerProvider(filePath));
            return builder;
        }
    }
}

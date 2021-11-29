using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using TestSystem.API.Extensions;

namespace TestSystem.API.Core.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerFactory loggerFactory;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
            this.loggerFactory = LoggerFactory.Create(builder =>
            {
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), "request_log.txt");

                builder.AddConsole()
                       .AddDebug()
                       .AddTextFile(logPath);
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogInformation("{0} - {1} {2} ({3})",
                                  DateTime.Now,
                                  context.Request.Method,
                                  context.Request.Path,
                                  context.Connection.RemoteIpAddress?.ToString()
                                    ?? "unknown IP");

            await next.Invoke(context);
        }
    }
}

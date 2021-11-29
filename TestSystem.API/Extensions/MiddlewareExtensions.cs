using Microsoft.AspNetCore.Builder;
using TestSystem.API.Core.Middleware;

namespace TestSystem.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
            => app.UseMiddleware<LoggingMiddleware>();
    }
}

using TriangleAPI.Middlewares;

namespace TriangleAPI.Extensions
{
    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}

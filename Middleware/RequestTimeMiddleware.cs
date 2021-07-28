using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Restaurant_API.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();

            await next.Invoke(context);

            _stopwatch.Stop();

            long ts = _stopwatch.ElapsedMilliseconds;

            if(ts > 4000)
                _logger.LogInformation("Request: " + context.Request.Method + " at " + context.Request.Path + " took longer than 4s (" + ts/1000 + ")");

        }
    }
}
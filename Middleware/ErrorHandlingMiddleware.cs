using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurant_API.Exceptions;

namespace Restaurant_API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ForbidException fe)
            {
                context.Response.StatusCode = 403;
            }
            catch (BadRequestException bre)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(bre.Message);
            }
            catch (NotFoundException nft)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nft.Message);
            }
            catch (System.Exception e) 
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong!");
            }
        }
    }
}
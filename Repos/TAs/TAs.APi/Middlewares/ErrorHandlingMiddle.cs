using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TAs.Domain.Errors;
using TAs.Domain.Result;
namespace TAs.APi.Middlewares
{
    public class ErrorHandlingMiddle : IExceptionHandler
    {
        // public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        // {
        //     try
        //     {
        //         await next.Invoke(context);
        //     }
        //     catch (NotFoundException notfound)
        //     {
        //         context.Response.StatusCode = 404;
        //         await context.Response.WriteAsync(notfound.Message);
        //         logger.LogError(notfound.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         logger.LogError(ex, ex.Message);
        //         context.Response.StatusCode = 500;
        //         await context.Response.WriteAsync(ex.Message);
        //     }
        // }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = new Error("Unhandled Exception", exception.Message);

            var result = Result.Failure(error);

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}
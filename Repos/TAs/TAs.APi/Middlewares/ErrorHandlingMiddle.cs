using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TAs.Domain.Errors;
using TAs.Domain.Exceptions;
using TAs.Domain.Result;
namespace TAs.APi.Middlewares;
public class ErrorHandlingMiddle : IExceptionHandler
{
    private readonly ILogger<ErrorHandlingMiddle> _logger;

    public ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new Error("Unhandled Exception", exception.Message);

        switch (exception)
        {
            case NotFoundException notFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new Error("Not Found", notFoundException.Message);
                _logger.LogWarning(exception, "Not Found: " + exception.Message);
                break;

            case UnauthorizedAccessException unauthorizedAccessException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = new Error("Unauthorized", unauthorizedAccessException.Message);
                _logger.LogWarning(exception, "Unauthorized: " + exception.Message);
                break;

            case ValidationException validationException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new Error("Validation Error", validationException.Message);
                _logger.LogWarning(exception, "Validation Failed: " + exception.Message);
                break;

            case Exception generalException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new Error("Internal Server Error", generalException.Message);
                _logger.LogError(exception, "General Exception: " + exception.Message);
                break;
        }

        var result = Result.Failure(errorResponse);

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true; 
    }
}

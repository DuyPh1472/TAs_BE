using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TAs.Domain.Errors;
using TAs.Domain.Exceptions;
using TAs.Domain.Result;

namespace TAs.APi.Middlewares
{
    public class ErrorHandlingMiddle : IExceptionHandler
    {
        private readonly ILogger<ErrorHandlingMiddle> _logger;

        public ErrorHandlingMiddle(ILogger<ErrorHandlingMiddle> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
        
            httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            httpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");


            httpContext.Response.ContentType = "application/json";

            // Thiết lập thông báo lỗi mặc định
            var errorResponse = new Error("Unexpected Error", "An unexpected error occurred.");

            switch (exception)
            {
                case NotFoundException notFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new Error("Not Found", "The requested resource could not be found.");
                    _logger.LogWarning(exception, "Not Found: " + exception.Message);
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse = new Error("Unauthorized", "You do not have permission to access this resource.");
                    _logger.LogWarning(exception, "Unauthorized: " + exception.Message);
                    break;

                case ValidationException validationException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new Error("Validation Error", "There was an issue with the provided input.");
                    _logger.LogWarning(exception, "Validation Failed: " + exception.Message);
                    break;

                case Exception generalException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new Error("Internal Server Error", "An internal server error occurred.");
                    _logger.LogError(exception, "Internal Server Error: " + exception.Message);
                    break;
            }

            // Trả về lỗi dưới dạng JSON
            var result = Result.Failure(errorResponse);
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}

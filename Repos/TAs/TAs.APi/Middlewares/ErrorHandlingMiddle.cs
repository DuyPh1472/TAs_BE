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

    // Cách thức middleware xử lý lỗi
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Mặc định nội dung trả về là application/json
        httpContext.Response.ContentType = "application/json";

        // Khởi tạo thông báo lỗi chung
        var errorResponse = new Error("Unhandled Exception", exception.Message);

        // Cập nhật mã trạng thái HTTP dựa trên loại lỗi
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

        // Trả về kết quả lỗi dưới dạng JSON
        var result = Result.Failure(errorResponse);

        // Trả về lỗi dưới dạng JSON cho client
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true; // Đánh dấu là lỗi đã được xử lý
    }
}

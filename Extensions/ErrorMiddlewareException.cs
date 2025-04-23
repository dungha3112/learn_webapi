using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

public class ErrorMiddlewareException
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorMiddlewareException> _logger;
    private readonly IHostEnvironment _env;

    public ErrorMiddlewareException(RequestDelegate next, ILogger<ErrorMiddlewareException> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;  // Default is 500
            var message = "An unexpected error occurred.";

            // Đặt thông báo và statusCode phù hợp
            if (ex is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;  // 401 Unauthorized
                message = ex.Message;
            }
            else if (ex is InvalidOperationException)
            {
                statusCode = HttpStatusCode.BadRequest;  // 400 Bad Request
                message = ex.Message;
            }
            else if (ex is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.BadRequest;  // 404 Bad Request
                message = ex.Message;
            }

            // Log lỗi
            _logger.LogError(ex, message);

            // Tạo ProblemDetails
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,  // Đảm bảo statusCode chính xác (401 hoặc 500)
                Title = statusCode == HttpStatusCode.Unauthorized ? "Unauthorized" : "Internal Server Error",  // Tùy theo mã lỗi
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Detail = "See the errors property for details.",
                Instance = context.Request.Path
            };

            // Nếu có lỗi, thêm vào trong "errors"
            problemDetails.Extensions.Add("errors", new { message });

            // Cấu hình response trả về
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }
    }
}

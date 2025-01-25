using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESkitNet.Core.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {ExceptionMessage}, Time of occurrence {Time}", exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) = exception switch
        {
            InternalServerException => BuildExceptionDetails(httpContext, exception, StatusCodes.Status500InternalServerError),
            ValidationException => BuildExceptionDetails(httpContext, exception, StatusCodes.Status400BadRequest),
            BadRequestException => BuildExceptionDetails(httpContext, exception, StatusCodes.Status400BadRequest),
            NotFoundException => BuildExceptionDetails(httpContext, exception, StatusCodes.Status404NotFound),
            _ => BuildExceptionDetails(httpContext, exception, StatusCodes.Status500InternalServerError)
        };

        var problemDetails = new ProblemDetails
        {
            Title = Title,
            Detail = Detail,
            Status = StatusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }

    private static (string, string, int) BuildExceptionDetails(HttpContext httpContext, Exception exception, int statusCode)
    {
        return (exception.Message, exception.GetType().Name, httpContext.Response.StatusCode = statusCode);
    }
}


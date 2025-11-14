using LiquidApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LiquidApi.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);

        switch (exception)
        {
            case EntityNotFoundException:
                {
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                }
            default:
                {
                    await problemDetailsService.WriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = httpContext,
                        Exception = exception,
                        ProblemDetails = new ProblemDetails
                        {
                            Title = "Internal Server Error",
                            Detail = "An error occured while processing the request",
                            Status = StatusCodes.Status500InternalServerError
                        }
                    });
                    break;
                }
        }

        return true;
    }
}

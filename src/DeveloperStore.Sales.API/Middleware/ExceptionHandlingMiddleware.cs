using DeveloperStore.Sales.Application.Common.Exceptions;
using DeveloperStore.Sales.Domain.Common;
using FluentValidation;

namespace DeveloperStore.Sales.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException ex => (
                StatusCodes.Status404NotFound,
                ex.Message,
                (IEnumerable<string>?)null),

            DomainException ex => (
                StatusCodes.Status422UnprocessableEntity,
                ex.Message,
                (IEnumerable<string>?)null),

            ValidationException ex => (
                StatusCodes.Status400BadRequest,
                "One or more validation errors occurred.",
                ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")),

            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                (IEnumerable<string>?)null)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message,
            errors
        });
    }
}

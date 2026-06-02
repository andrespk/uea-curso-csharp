using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniKanban.Application.Interfaces.Auth;
using MiniKanban.Exceptions;

namespace MiniKanban.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
        var invalidOrExpiredTokenDetails = new InvalidOrExpiredTokenException().Message;
        var logId = Guid.NewGuid().ToString("N").Substring(0, 10);


        if (!string.IsNullOrEmpty(authorizationHeader) &&
            authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var tokenService = httpContext.RequestServices.GetRequiredService<ITokenService>();

            if (!tokenService.ValidateToken(token))
            {
                _logger.LogWarning("[ALERTA][{LogId}] {InvalidOrExpiredTokenDetails}", logId,
                    invalidOrExpiredTokenDetails);

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = StringifyProblemDetails(logId, invalidOrExpiredTokenDetails)
                }, cancellationToken);

                return true;
            }
        }

        var isBusinessOrValidationException = exception is BusinessException || exception is ValidationException;
        var isTokenException = exception is SecurityTokenException;
        var isCancellationException = exception is OperationCanceledException || exception is TaskCanceledException;

        if (isCancellationException)
        {
            _logger.LogInformation("[ALERTA][{LogId}] A operação foi cancelada pelo cliente.", logId);
            try
            {
                httpContext.Response.StatusCode = 499; // Client Closed Request
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 499,
                    Title = "Client Closed Request",
                    Detail = StringifyProblemDetails(logId, "A operação foi cancelada.")
                }, cancellationToken);
            }
            catch
            {
                // Ignora falhas de escrita se a conexão já estiver fechada
            }

            return true;
        }

        if (isTokenException)
        {
            _logger.LogWarning("[ALERTA][{LogId}] {Name} {ExceptionMessage}", nameof(SecurityTokenException), logId,
                exception.Message);
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = StringifyProblemDetails(logId, invalidOrExpiredTokenDetails)
            }, cancellationToken);

            return true;
        }

        if (!isBusinessOrValidationException)
        {
            var message =
                $"[ERRO][{logId}] {exception.GetType().Name}: {exception.Message} | Stack Trace: {exception.StackTrace}";
            _logger.LogError(exception, message);

            if (exception.InnerException != null)
            {
                var innerMessage =
                    $"[ERRO INTERNO][{logId}] {exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
                _logger.LogError(innerMessage);
            }

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = StringifyProblemDetails(logId, "Erro interno no servidor.")
            }, cancellationToken);
        }
        else
        {
            _logger.LogWarning("[ALERTA][{LogId}] {ExceptionMessage}", logId, exception.Message);
            if (exception.InnerException != null)
                _logger.LogWarning(
                    "[ALERTA INTERNO][{LogId}] {InnerExceptionMessage}", logId, exception.InnerException.Message);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var message = exception is ValidationException validationException
                ? string.Join("; ",
                    validationException.ValidationResult.MemberNames.Select(m =>
                        $"{m}: {validationException.ValidationResult.ErrorMessage}"))
                : exception.Message;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = StringifyProblemDetails(logId, message)
            }, cancellationToken);
        }

        return true;
    }

    private static string StringifyProblemDetails(string logId, string message)
    {
        return JsonSerializer.Serialize(new { logId, message });
    }
}
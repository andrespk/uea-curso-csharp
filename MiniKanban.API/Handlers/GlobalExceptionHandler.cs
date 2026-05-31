using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MiniKanban.Application.Interfaces;
using MiniKanban.Exceptions;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var tokenService = httpContext.RequestServices.GetRequiredService<ITokenService>();

            if (!tokenService.ValidateToken(token))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[ALERTA] Token inválido detectado no ExceptionHandler");
                Console.ResetColor();

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = "Token de autenticação inválido ou expirado."
                }, cancellationToken);

                return true;
            }
        }

        var isBusinessOrValidation = exception is BusinessException || exception is ValidationError;
        var isTokenException = exception is SecurityTokenException;

        if (isTokenException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ALERTA] Falha de token: {exception.Message}");
            if (exception.InnerException != null)
            {
                Console.WriteLine($"[ALERTA] Exceção Interna: {exception.InnerException.Message}");
            }
            Console.ResetColor();

            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Token de autenticação inválido ou expirado."
            }, cancellationToken);

            return true;
        }

        if (!isBusinessOrValidation)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERRO] Exceção: {exception.Message}");
            Console.WriteLine($"Stack Trace: {exception.StackTrace}");
            if (exception.InnerException != null)
            {
                Console.WriteLine($"Exceção Interna: {exception.InnerException.Message}");
            }
            Console.ResetColor();

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            }, cancellationToken);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ALERTA] Exceção: {exception.Message}");
            if (exception.InnerException != null)
            {
                Console.WriteLine($"[ALERTA] Exceção Interna: {exception.InnerException.Message}");
            }
            Console.ResetColor();

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = exception.Message
            }, cancellationToken);
        }

        return true;
    }
}

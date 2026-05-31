using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async ([FromBody] LoginRequestDto request, ILoginService loginService) =>
        {
            var result = await loginService.LoginAsync(request);
            if (result == null)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(result);
        })
        .WithName("Login")
        .WithSummary("Realiza o login do usuário")
        .WithDescription("Autentica o usuário no sistema com base no Username e Password, retornando as informações do usuário e o token JWT correspondente.")
        .WithTags("Autenticação")
        .Produces<LoginResponseDto>(200)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/protected", () =>
        {
            return Results.Ok(new { Message = "Access granted to protected endpoint!" });
        })
        .RequireAuthorization()
        .WithName("Protected")
        .WithSummary("Acessa endpoint protegido")
        .WithDescription("Retorna uma mensagem de confirmação para testar se o token JWT fornecido no cabeçalho Authorization é válido.")
        .WithTags("Autenticação")
        .Produces(200)
        .Produces(401)
        .WithOpenApi();
    }
}

using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async ([FromBody] LoginRequestDto request, ILoginService loginService, CancellationToken cancellationToken) =>
        {
            var result = await loginService.LoginAsync(request, cancellationToken);
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

        app.MapPost("/api/auth/register", async ([FromBody] CreateUserDto request, IRegisterUserService registerUserService, CancellationToken cancellationToken) =>
        {
            var result = await registerUserService.RegisterAsync(request, cancellationToken);
            return Results.Created($"/api/users/{result.Id}", result);
        })
        .WithName("Register")
        .WithSummary("Registra um novo usuário")
        .WithDescription("Cria uma conta de usuário com senha armazenada em hash.")
        .WithTags("Autenticação")
        .Produces<UserResponseDto>(201)
        .Produces(400)
        .WithOpenApi();

        app.MapGet("/api/me", async (HttpContext httpContext, IGetUserByIdService getUserByIdService, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await getUserByIdService.GetByIdAsync(userId.Value, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithSummary("Retorna o usuário autenticado")
        .WithDescription("Retorna os dados do usuário identificado pelo token JWT.")
        .WithTags("Autenticação")
        .Produces<UserResponseDto>(200)
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

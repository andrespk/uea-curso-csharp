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
        .WithOpenApi();

        app.MapGet("/api/protected", () =>
        {
            return Results.Ok(new { Message = "Access granted to protected endpoint!" });
        })
        .RequireAuthorization()
        .WithName("Protected")
        .WithOpenApi();
    }
}

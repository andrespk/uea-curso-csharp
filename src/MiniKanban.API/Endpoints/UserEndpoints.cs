using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.User;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (IGetUsersService getUsersService, CancellationToken cancellationToken) =>
            {
                var result = await getUsersService.GetAllAsync(cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetUsers")
            .WithSummary("Lista usuários")
            .WithDescription("Retorna todos os usuarios cadastrados sem expor o hash de senha.")
            .WithTags("Usuários")
            .Produces<IEnumerable<UserResponseDto>>()
            .Produces(401)
            .WithOpenApi();

        app.MapGet("/api/users/{id:guid}",
                async (Guid id, IGetUserByIdService getUserByIdService, CancellationToken cancellationToken) =>
                {
                    var result = await getUserByIdService.GetByIdAsync(id, cancellationToken);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetUserById")
            .WithSummary("Busca usuário por ID")
            .WithDescription("Retorna um usuario pelo identificador sem expor o hash de senha.")
            .WithTags("Usuários")
            .Produces<UserResponseDto>()
            .Produces(400)
            .Produces(401)
            .WithOpenApi();
    }
}
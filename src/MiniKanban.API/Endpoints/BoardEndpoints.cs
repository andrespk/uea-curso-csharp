using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class BoardEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/boards", async (
            HttpContext httpContext,
            [FromBody] CreateBoardDto request,
            ICreateBoardService createBoardService,
            CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            request.OwnerId = userId.Value;
            var result = await createBoardService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/boards/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateBoard")
        .WithSummary("Cria board")
        .WithDescription("Cria um novo board para o usuario autenticado. O OwnerId e obtido do token JWT, mesmo que venha no corpo da requisicao.")
        .WithTags("Boards")
        .Produces<BoardResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/boards", async (
            HttpContext httpContext,
            IGetBoardsByUserService getBoardsByUserService,
            CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await getBoardsByUserService.GetByUserIdAsync(userId.Value, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetMyBoards")
        .WithSummary("Lista boards do usuário autenticado")
        .WithDescription("Retorna os boards em que o usuario identificado pelo token JWT participa como membro.")
        .WithTags("Boards")
        .Produces<IEnumerable<BoardResponseDto>>(200)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/boards/{id:guid}", async (Guid id, IGetBoardByIdService getBoardByIdService, CancellationToken cancellationToken) =>
        {
            var result = await getBoardByIdService.GetByIdAsync(id, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetBoardById")
        .WithSummary("Busca board por ID")
        .WithDescription("Retorna os dados basicos de um board pelo identificador.")
        .WithTags("Boards")
        .Produces<BoardResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/users/{ownerId:guid}/boards/owned", async (
            Guid ownerId,
            IGetBoardsByOwnerService getBoardsByOwnerService,
            CancellationToken cancellationToken) =>
        {
            var result = await getBoardsByOwnerService.GetByOwnerIdAsync(ownerId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetBoardsByOwner")
        .WithSummary("Lista boards criados por usuário")
        .WithDescription("Retorna os boards cujo owner corresponds ao usuario informado na rota.")
        .WithTags("Boards")
        .Produces<IEnumerable<BoardResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/boards/{id:guid}", async (
            Guid id,
            [FromBody] UpdateBoardDto request,
            IUpdateBoardService updateBoardService,
            CancellationToken cancellationToken) =>
        {
            var result = await updateBoardService.UpdateAsync(id, request, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateBoard")
        .WithSummary("Atualiza board")
        .WithDescription("Atualiza nome e descricao de um board existente.")
        .WithTags("Boards")
        .Produces<BoardResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/boards/{id:guid}", async (Guid id, IDeleteBoardService deleteBoardService, CancellationToken cancellationToken) =>
        {
            await deleteBoardService.DeleteAsync(id, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteBoard")
        .WithSummary("Remove board")
        .WithDescription("Remove um board existente e seus relacionamentos configurados com cascade no EF Core.")
        .WithTags("Boards")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}

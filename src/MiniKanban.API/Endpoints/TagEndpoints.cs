using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class TagEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tags", async (
            [FromBody] CreateTagDto request,
            ICreateTagService createTagService,
            CancellationToken cancellationToken) =>
        {
            var result = await createTagService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/tags/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateTag")
        .WithSummary("Cria tag")
        .WithDescription("Cria uma nova tag em um board.")
        .WithTags("Tags")
        .Produces<TagResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/boards/{boardId:guid}/tags", async (
            Guid boardId,
            IGetTagsByBoardService getTagsByBoardService,
            CancellationToken cancellationToken) =>
        {
            var result = await getTagsByBoardService.GetByBoardIdAsync(boardId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetTagsByBoard")
        .WithSummary("Lista tags de um board")
        .WithDescription("Retorna todas as tags associadas a um board.")
        .WithTags("Tags")
        .Produces<IEnumerable<TagResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/tags/{id:guid}", async (
            Guid id,
            [FromBody] UpdateTagDto request,
            IUpdateTagService updateTagService,
            CancellationToken cancellationToken) =>
        {
            var result = await updateTagService.UpdateAsync(id, request, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateTag")
        .WithSummary("Atualiza tag")
        .WithDescription("Atualiza os dados de uma tag existente.")
        .WithTags("Tags")
        .Produces<TagResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/tags/{id:guid}", async (
            Guid id,
            IDeleteTagService deleteTagService,
            CancellationToken cancellationToken) =>
        {
            await deleteTagService.DeleteAsync(id, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteTag")
        .WithSummary("Deleta tag")
        .WithDescription("Remove uma tag existente.")
        .WithTags("Tags")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}


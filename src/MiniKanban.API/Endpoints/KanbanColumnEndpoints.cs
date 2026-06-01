using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class KanbanColumnEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/kanban-columns", async (
            [FromBody] CreateKanbanColumnDto request,
            ICreateKanbanColumnService createKanbanColumnService,
            CancellationToken cancellationToken) =>
        {
            var result = await createKanbanColumnService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/kanban-columns/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateKanbanColumn")
        .WithSummary("Cria coluna Kanban")
        .WithDescription("Cria uma coluna em um board existente. Valida ordem nao negativa, WIP limit nao negativo e ordem unica dentro do board.")
        .WithTags("Colunas Kanban")
        .Produces<KanbanColumnResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/boards/{boardId:guid}/kanban-columns", async (
            Guid boardId,
            IGetKanbanColumnsByBoardService getKanbanColumnsByBoardService,
            CancellationToken cancellationToken) =>
        {
            var result = await getKanbanColumnsByBoardService.GetByBoardIdAsync(boardId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetKanbanColumnsByBoard")
        .WithSummary("Lista colunas do board")
        .WithDescription("Retorna as colunas de um board ordenadas pelo campo Order.")
        .WithTags("Colunas Kanban")
        .Produces<IEnumerable<KanbanColumnResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/kanban-columns/{id:guid}", async (
            Guid id,
            [FromBody] UpdateKanbanColumnDto request,
            IUpdateKanbanColumnService updateKanbanColumnService,
            CancellationToken cancellationToken) =>
        {
            var result = await updateKanbanColumnService.UpdateAsync(id, request, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateKanbanColumn")
        .WithSummary("Atualiza coluna Kanban")
        .WithDescription("Atualiza nome, ordem e WIP limit de uma coluna Kanban existente.")
        .WithTags("Colunas Kanban")
        .Produces<KanbanColumnResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/kanban-columns/{id:guid}", async (
            Guid id,
            IDeleteKanbanColumnService deleteKanbanColumnService,
            CancellationToken cancellationToken) =>
        {
            await deleteKanbanColumnService.DeleteAsync(id, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteKanbanColumn")
        .WithSummary("Remove coluna Kanban")
        .WithDescription("Remove uma coluna Kanban existente.")
        .WithTags("Colunas Kanban")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}

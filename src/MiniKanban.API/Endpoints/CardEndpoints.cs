using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class CardEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/cards", async (
            HttpContext httpContext,
            [FromBody] CreateCardDto request,
            ICreateCardService createCardService) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            request.CreatedByUserId = userId.Value;
            var result = await createCardService.CreateAsync(request);
            return Results.Created($"/api/cards/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateCard")
        .WithSummary("Cria card")
        .WithDescription("Cria um novo card em uma coluna existente. O CreatedByUserId e obtido do token JWT.")
        .WithTags("Cards")
        .Produces<CardResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/columns/{columnId:guid}/cards", async (
            Guid columnId,
            IGetCardsByColumnService getCardsByColumnService) =>
        {
            var result = await getCardsByColumnService.GetByColumnIdAsync(columnId);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetCardsByColumn")
        .WithSummary("Lista cards de uma coluna")
        .WithDescription("Retorna os cards vinculados a coluna informada.")
        .WithTags("Cards")
        .Produces<IEnumerable<CardResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/cards/{id:guid}", async (
            Guid id,
            [FromBody] UpdateCardDto request,
            IUpdateCardService updateCardService) =>
        {
            var result = await updateCardService.UpdateAsync(id, request);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateCard")
        .WithSummary("Atualiza card")
        .WithDescription("Atualiza os dados de um card existente.")
        .WithTags("Cards")
        .Produces<CardResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}

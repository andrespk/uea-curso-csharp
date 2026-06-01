using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class CardTagEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/card-tags", async (
            [FromBody] CreateCardTagDto request,
            IAddCardTagService addCardTagService,
            CancellationToken cancellationToken) =>
        {
            var result = await addCardTagService.AddAsync(request, cancellationToken);
            return Results.Created($"/api/cards/{result.CardId}/tags/{result.TagId}", result);
        })
        .RequireAuthorization()
        .WithName("AddCardTag")
        .WithSummary("Associa tag a um card")
        .WithDescription("Adiciona uma tag existente a um card existente.")
        .WithTags("Card Tags")
        .Produces<CardTagResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/cards/{cardId:guid}/tags", async (
            Guid cardId,
            IGetCardTagsService getCardTagsService,
            CancellationToken cancellationToken) =>
        {
            var result = await getCardTagsService.GetByCardIdAsync(cardId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetCardTags")
        .WithSummary("Lista tags de um card")
        .WithDescription("Retorna todas as tags associadas a um card.")
        .WithTags("Card Tags")
        .Produces<IEnumerable<CardTagResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/cards/{cardId:guid}/tags/{tagId:guid}", async (
            Guid cardId,
            Guid tagId,
            IRemoveCardTagService removeCardTagService,
            CancellationToken cancellationToken) =>
        {
            await removeCardTagService.RemoveAsync(cardId, tagId, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("RemoveCardTag")
        .WithSummary("Remove tag de um card")
        .WithDescription("Remove a associação entre uma tag e um card.")
        .WithTags("Card Tags")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}


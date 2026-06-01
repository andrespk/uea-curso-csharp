using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class CommentEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/comments", async (
            HttpContext httpContext,
            [FromBody] CreateCommentDto request,
            ICreateCommentService createCommentService,
            CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            request.UserId = userId.Value;
            var result = await createCommentService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/comments/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("CreateComment")
        .WithSummary("Cria comentário")
        .WithDescription("Cria um novo comentário em um card. O UserId é obtido do token JWT.")
        .WithTags("Comments")
        .Produces<CommentResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/cards/{cardId:guid}/comments", async (
            Guid cardId,
            IGetCommentsByCardService getCommentsByCardService,
            CancellationToken cancellationToken) =>
        {
            var result = await getCommentsByCardService.GetByCardIdAsync(cardId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetCommentsByCard")
        .WithSummary("Lista comentários de um card")
        .WithDescription("Retorna todos os comentários associados a um card.")
        .WithTags("Comments")
        .Produces<IEnumerable<CommentResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/comments/{id:guid}", async (
            Guid id,
            [FromBody] UpdateCommentDto request,
            IUpdateCommentService updateCommentService,
            CancellationToken cancellationToken) =>
        {
            var result = await updateCommentService.UpdateAsync(id, request, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateComment")
        .WithSummary("Atualiza comentário")
        .WithDescription("Atualiza o conteúdo de um comentário existente.")
        .WithTags("Comments")
        .Produces<CommentResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/comments/{id:guid}", async (
            Guid id,
            IDeleteCommentService deleteCommentService,
            CancellationToken cancellationToken) =>
        {
            await deleteCommentService.DeleteAsync(id, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("DeleteComment")
        .WithSummary("Deleta comentário")
        .WithDescription("Remove um comentário existente.")
        .WithTags("Comments")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}


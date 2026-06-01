using Microsoft.AspNetCore.Mvc;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.API.Endpoints;

public class BoardMemberEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/board-members", async (
            [FromBody] CreateBoardMemberDto request,
            IAddBoardMemberService addBoardMemberService,
            CancellationToken cancellationToken) =>
        {
            var result = await addBoardMemberService.AddAsync(request, cancellationToken);
            return Results.Created($"/api/board-members/{result.Id}", result);
        })
        .RequireAuthorization()
        .WithName("AddBoardMember")
        .WithSummary("Adiciona membro ao board")
        .WithDescription("Adiciona um usuario existente como membro de um board existente. Nao permite duplicidade nem criacao manual de Owner.")
        .WithTags("Membros de Board")
        .Produces<BoardMemberResponseDto>(201)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapGet("/api/boards/{boardId:guid}/members", async (
            Guid boardId,
            IGetBoardMembersService getBoardMembersService,
            CancellationToken cancellationToken) =>
        {
            var result = await getBoardMembersService.GetByBoardIdAsync(boardId, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetBoardMembers")
        .WithSummary("Lista membros do board")
        .WithDescription("Retorna todos os membros vinculados ao board informado.")
        .WithTags("Membros de Board")
        .Produces<IEnumerable<BoardMemberResponseDto>>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapPut("/api/board-members/{id:guid}", async (
            Guid id,
            [FromBody] UpdateBoardMemberDto request,
            IUpdateBoardMemberService updateBoardMemberService,
            CancellationToken cancellationToken) =>
        {
            var result = await updateBoardMemberService.UpdateAsync(id, request, cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateBoardMember")
        .WithSummary("Atualiza papel do membro")
        .WithDescription("Atualiza o papel de um membro do board. A role Owner nao pode ser atribuida ou removida por este endpoint.")
        .WithTags("Membros de Board")
        .Produces<BoardMemberResponseDto>(200)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();

        app.MapDelete("/api/board-members/{id:guid}", async (
            Guid id,
            IRemoveBoardMemberService removeBoardMemberService,
            CancellationToken cancellationToken) =>
        {
            await removeBoardMemberService.RemoveAsync(id, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithName("RemoveBoardMember")
        .WithSummary("Remove membro do board")
        .WithDescription("Remove um membro do board. O membro com role Owner nao pode ser removido por este endpoint.")
        .WithTags("Membros de Board")
        .Produces(204)
        .Produces(400)
        .Produces(401)
        .WithOpenApi();
    }
}

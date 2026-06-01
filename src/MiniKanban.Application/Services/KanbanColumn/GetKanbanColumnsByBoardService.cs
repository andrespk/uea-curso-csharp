using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.KanbanColumn;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.KanbanColumn;

public class GetKanbanColumnsByBoardService : IGetKanbanColumnsByBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;

    public GetKanbanColumnsByBoardService(IKanbanColumnRepository kanbanColumnRepository,
        IBoardRepository boardRepository)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<KanbanColumnResponseDto>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(boardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var columns = await _kanbanColumnRepository.GetByBoardIdAsync(boardId, cancellationToken);
        return columns.Select(KanbanColumnMapping.ToResponse);
    }
}
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetKanbanColumnsByBoardService : IGetKanbanColumnsByBoardService, ScopedInjection
{
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IBoardRepository _boardRepository;

    public GetKanbanColumnsByBoardService(IKanbanColumnRepository kanbanColumnRepository, IBoardRepository boardRepository)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<KanbanColumnResponseDto>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(boardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var columns = await _kanbanColumnRepository.GetByBoardIdAsync(boardId, cancellationToken);
        return columns.Select(KanbanColumnMapping.ToResponse);
    }
}

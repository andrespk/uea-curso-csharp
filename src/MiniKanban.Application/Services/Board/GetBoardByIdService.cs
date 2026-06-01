using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetBoardByIdService : IGetBoardByIdService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdService(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<BoardResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var board = await _boardRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Board not found.");

        return BoardMapping.ToResponse(board);
    }
}

using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateBoardService : IUpdateBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardService(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var board = await _boardRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Board not found.");

        cancellationToken.ThrowIfCancellationRequested();

        BoardMapping.ToEntity(request, board);

        board.UpdatedAt = DateTime.UtcNow;

        await _boardRepository.UpdateAsync(board, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BoardMapping.ToResponse(board);
    }
}

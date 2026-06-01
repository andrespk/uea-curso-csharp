using MiniKanban.Application.Interfaces.Board;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Board;

public class DeleteBoardService : IDeleteBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardService(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var board = await _boardRepository.GetByIdAsync(id, cancellationToken)
                    ?? throw new BusinessException("Board not found.");

        cancellationToken.ThrowIfCancellationRequested();

        await _boardRepository.DeleteAsync(board, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
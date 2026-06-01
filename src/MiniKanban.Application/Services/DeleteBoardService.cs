using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class DeleteBoardService : IDeleteBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardService(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id)
    {
        var board = await _boardRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Board not found.");

        await _boardRepository.DeleteAsync(board);
        await _unitOfWork.CommitAsync();
    }
}

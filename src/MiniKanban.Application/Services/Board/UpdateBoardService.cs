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

    public async Task<BoardResponseDto> UpdateAsync(Guid id, UpdateBoardDto request)
    {
        var board = await _boardRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Board not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))
            board.Name = request.Name;

        if (request.Description != null)
            board.Description = request.Description;

        board.UpdatedAt = DateTime.UtcNow;

        await _boardRepository.UpdateAsync(board);
        await _unitOfWork.CommitAsync();

        return BoardMapping.ToResponse(board);
    }
}

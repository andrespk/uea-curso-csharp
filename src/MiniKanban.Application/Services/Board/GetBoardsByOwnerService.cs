using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Board;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Board;

public class GetBoardsByOwnerService : IGetBoardsByOwnerService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;

    public GetBoardsByOwnerService(IBoardRepository boardRepository, IUserRepository userRepository)
    {
        _boardRepository = boardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<BoardResponseDto>> GetByOwnerIdAsync(Guid ownerId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _userRepository.GetByIdAsync(ownerId, cancellationToken) == null)
            throw new BusinessException("Owner user not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var boards = await _boardRepository.GetByOwnerIdAsync(ownerId, cancellationToken);
        return boards.Select(BoardMapping.ToResponse);
    }
}
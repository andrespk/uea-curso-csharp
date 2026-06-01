using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Board;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Board;

public class GetBoardsByUserService : IGetBoardsByUserService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;

    public GetBoardsByUserService(IBoardRepository boardRepository, IUserRepository userRepository)
    {
        _boardRepository = boardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<BoardResponseDto>> GetByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _userRepository.GetByIdAsync(userId, cancellationToken) == null)
            throw new BusinessException("User not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var boards = await _boardRepository.GetByMemberUserIdAsync(userId, cancellationToken);
        return boards.Select(BoardMapping.ToResponse);
    }
}
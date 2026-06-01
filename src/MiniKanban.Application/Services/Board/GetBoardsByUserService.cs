using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetBoardsByUserService : IGetBoardsByUserService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;

    public GetBoardsByUserService(IBoardRepository boardRepository, IUserRepository userRepository)
    {
        _boardRepository = boardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<BoardResponseDto>> GetByUserIdAsync(Guid userId)
    {
        if (await _userRepository.GetByIdAsync(userId) == null)
            throw new BusinessException("User not found.");

        var boards = await _boardRepository.GetByMemberUserIdAsync(userId);
        return boards.Select(BoardMapping.ToResponse);
    }
}

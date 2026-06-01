using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetBoardsByOwnerService : IGetBoardsByOwnerService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;

    public GetBoardsByOwnerService(IBoardRepository boardRepository, IUserRepository userRepository)
    {
        _boardRepository = boardRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<BoardResponseDto>> GetByOwnerIdAsync(Guid ownerId)
    {
        if (await _userRepository.GetByIdAsync(ownerId) == null)
            throw new BusinessException("Owner user not found.");

        var boards = await _boardRepository.GetByOwnerIdAsync(ownerId);
        return boards.Select(BoardMapping.ToResponse);
    }
}

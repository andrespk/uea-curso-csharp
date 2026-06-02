using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.User;

public interface IGetUsersService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
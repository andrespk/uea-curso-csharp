using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetUsersService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
}

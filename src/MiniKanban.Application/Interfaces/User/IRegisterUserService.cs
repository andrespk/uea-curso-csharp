using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.User;

public interface IRegisterUserService
{
    Task<UserResponseDto> RegisterAsync(CreateUserDto request, CancellationToken cancellationToken = default);
}
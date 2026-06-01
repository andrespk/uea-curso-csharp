using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IRegisterUserService
{
    Task<UserResponseDto> RegisterAsync(CreateUserDto request);
}

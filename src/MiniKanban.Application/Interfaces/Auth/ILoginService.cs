using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Auth;

public interface ILoginService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
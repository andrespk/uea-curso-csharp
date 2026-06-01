using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ILoginService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}

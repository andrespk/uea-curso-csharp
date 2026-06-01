using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.User;

public interface IGetUserByIdService
{
    Task<UserResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
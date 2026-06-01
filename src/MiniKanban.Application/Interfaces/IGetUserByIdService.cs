using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IGetUserByIdService
{
    Task<UserResponseDto> GetByIdAsync(Guid id);
}

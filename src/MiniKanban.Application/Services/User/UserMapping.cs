using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class UserMapping
{
    public static UserResponseDto ToResponse(User user)
    {
        return user.Adapt<UserResponseDto>();
    }
}

using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Helpers;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class UserMapping
{
    public static UserResponseDto ToResponse(User user)
    {
        return user.Adapt<UserResponseDto>();
    }

    public static User ToEntity(CreateUserDto dto)
    {
        var user = dto.Adapt<User>();
        user.PasswordHash = PasswordHasher.Hash(dto.Password);
        if (string.IsNullOrWhiteSpace(user.Role))
        {
            user.Role = "User";
        }
        return user;
    }

    public static User ToEntity(UpdateUserDto dto, User entity)
    {
        dto.Adapt(entity);
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            entity.PasswordHash = PasswordHasher.Hash(dto.Password);
        }
        return entity;
    }
}

using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Infrastructure.Helpers;

namespace MiniKanban.Application.Services.User;

internal static class UserMapping
{
    public static UserResponseDto ToResponse(Domain.Entities.User user)
    {
        return user.Adapt<UserResponseDto>();
    }

    public static Domain.Entities.User ToEntity(CreateUserDto dto)
    {
        var user = dto.Adapt<Domain.Entities.User>();
        user.PasswordHash = PasswordHasher.Hash(dto.Password);
        if (string.IsNullOrWhiteSpace(user.Role)) user.Role = "User";
        return user;
    }

    public static Domain.Entities.User ToEntity(UpdateUserDto dto, Domain.Entities.User entity)
    {
        dto.Adapt(entity);
        if (!string.IsNullOrWhiteSpace(dto.Password)) entity.PasswordHash = PasswordHasher.Hash(dto.Password);
        return entity;
    }
}
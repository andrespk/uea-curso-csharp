using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetUserByIdService : IGetUserByIdService, ScopedInjection
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new BusinessException("User not found.");

        return UserMapping.ToResponse(user);
    }
}

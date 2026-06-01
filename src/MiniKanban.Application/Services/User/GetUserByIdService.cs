using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.User;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.User;

public class GetUserByIdService : IGetUserByIdService, ScopedInjection
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                   ?? throw new BusinessException("User not found.");

        return UserMapping.ToResponse(user);
    }
}
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Application.Services;

public class GetUsersService : IGetUsersService, ScopedInjection
{
    private readonly IUserRepository _userRepository;

    public GetUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(UserMapping.ToResponse);
    }
}

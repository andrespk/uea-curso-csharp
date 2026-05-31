using MiniKanban.Application.DTOs;
using MiniKanban.Application.Helpers;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;

namespace MiniKanban.Application.Services;

public class LoginService : ILoginService, ScopedInjection
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var token = _tokenService.GenerateToken(user);
        return new LoginResponseDto
        {
            Username = user.Username,
            Token = token
        };
    }
}

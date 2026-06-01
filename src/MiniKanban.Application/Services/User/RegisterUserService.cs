using MiniKanban.Application.DTOs;
using MiniKanban.Application.Helpers;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class RegisterUserService : IRegisterUserService, ScopedInjection
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponseDto> RegisterAsync(CreateUserDto request)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username))
            throw new BusinessException("Username already exists.");

        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new BusinessException("Email already exists.");

        var user = new User
        {
            Name = request.Name,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return UserMapping.ToResponse(user);
    }
}

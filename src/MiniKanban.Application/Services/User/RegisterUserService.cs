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

    public async Task<UserResponseDto> RegisterAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            throw new BusinessException("Username already exists.");

        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
            throw new BusinessException("Email already exists.");

        cancellationToken.ThrowIfCancellationRequested();

        var user = UserMapping.ToEntity(request);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return UserMapping.ToResponse(user);
    }
}

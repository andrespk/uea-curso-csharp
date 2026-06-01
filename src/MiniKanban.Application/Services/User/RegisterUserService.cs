using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.User;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.User;

public class RegisterUserService : IRegisterUserService, ScopedInjection
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RegisterUserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponseDto> RegisterAsync(CreateUserDto request,
        CancellationToken cancellationToken = default)
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
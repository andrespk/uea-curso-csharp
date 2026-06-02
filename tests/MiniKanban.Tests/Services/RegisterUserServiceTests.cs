using MiniKanban.Application.DTOs;
using MiniKanban.Application.Services.User;
using MiniKanban.Domain.Entities;
using MiniKanban.Exceptions;
using MiniKanban.Tests.Fakes;

namespace MiniKanban.Tests.Services;

public class RegisterUserServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenDataIsValid_CreatesUserAndCommits()
    {
        var userRepository = new FakeUserRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new RegisterUserService(userRepository, unitOfWork);

        var result = await service.RegisterAsync(new CreateUserDto
        {
            Name = "Maria Silva",
            Username = "maria",
            Email = "maria@example.com",
            Password = "Password123",
            Role = "User"
        });

        var createdUser = Assert.Single(userRepository.SavedItems);
        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("Maria Silva", result.Name);
        Assert.Equal("maria", result.Username);
        Assert.NotEqual("Password123", createdUser.PasswordHash);
        Assert.Equal(1, unitOfWork.CommitCount);
    }

    [Fact]
    public async Task RegisterAsync_WhenUsernameAlreadyExists_ThrowsBusinessException()
    {
        var userRepository = new FakeUserRepository(new[]
        {
            new User
            {
                Name = "Maria", Username = "maria", Email = "maria@example.com", PasswordHash = "hash", Role = "User"
            }
        });
        var service = new RegisterUserService(userRepository, new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.RegisterAsync(new CreateUserDto
        {
            Name = "Maria Dois",
            Username = "maria",
            Email = "maria2@example.com",
            Password = "Password123",
            Role = "User"
        }));
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsBusinessException()
    {
        var userRepository = new FakeUserRepository(new[]
        {
            new User
            {
                Name = "Maria", Username = "maria", Email = "maria@example.com", PasswordHash = "hash", Role = "User"
            }
        });
        var service = new RegisterUserService(userRepository, new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.RegisterAsync(new CreateUserDto
        {
            Name = "Joao",
            Username = "joao",
            Email = "maria@example.com",
            Password = "Password123",
            Role = "User"
        }));
    }
}
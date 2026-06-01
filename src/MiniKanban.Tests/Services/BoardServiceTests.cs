using MiniKanban.Application.DTOs;
using MiniKanban.Application.Services;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Enums;
using MiniKanban.Exceptions.Users;
using MiniKanban.Tests.Fakes;

namespace MiniKanban.Tests.Services;

public class BoardServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenOwnerExists_CreatesBoardAndOwnerMembership()
    {
        var owner = new User { Name = "Owner", Username = "owner", Email = "owner@example.com", PasswordHash = "hash", Role = "User" };
        var boardRepository = new FakeBoardRepository();
        var memberRepository = new FakeBoardMemberRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new CreateBoardService(
            boardRepository,
            memberRepository,
            new FakeUserRepository(new[] { owner }),
            unitOfWork);

        var result = await service.CreateAsync(new CreateBoardDto
        {
            Name = "Projeto",
            Description = "Kanban do projeto",
            OwnerId = owner.Id
        });

        var board = Assert.Single(boardRepository.SavedItems);
        var ownerMember = Assert.Single(memberRepository.SavedItems);
        Assert.Equal(board.Id, result.Id);
        Assert.Equal(owner.Id, board.OwnerId);
        Assert.Equal(board.Id, ownerMember.BoardId);
        Assert.Equal(owner.Id, ownerMember.UserId);
        Assert.Equal(BoardRole.Owner, ownerMember.Role);
        Assert.Equal(1, unitOfWork.CommitCount);
    }

    [Fact]
    public async Task CreateAsync_WhenOwnerDoesNotExist_ThrowsBusinessException()
    {
        var service = new CreateBoardService(
            new FakeBoardRepository(),
            new FakeBoardMemberRepository(),
            new FakeUserRepository(),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.CreateAsync(new CreateBoardDto
        {
            Name = "Projeto",
            OwnerId = Guid.NewGuid()
        }));
    }

    [Fact]
    public async Task UpdateAsync_WhenBoardExists_UpdatesNameDescriptionAndCommits()
    {
        var board = new Board { Name = "Antigo", Description = "Antes", OwnerId = Guid.NewGuid() };
        var boardRepository = new FakeBoardRepository(new[] { board });
        var unitOfWork = new FakeUnitOfWork();
        var service = new UpdateBoardService(boardRepository, unitOfWork);

        var result = await service.UpdateAsync(board.Id, new UpdateBoardDto
        {
            Name = "Novo",
            Description = "Depois"
        });

        Assert.Equal("Novo", result.Name);
        Assert.Equal("Depois", result.Description);
        Assert.NotNull(board.UpdatedAt);
        Assert.Equal(1, unitOfWork.CommitCount);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBoardDoesNotExist_ThrowsBusinessException()
    {
        var service = new GetBoardByIdService(new FakeBoardRepository());

        await Assert.ThrowsAsync<BusinessException>(() => service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAsync_WhenBoardExists_RemovesBoardAndCommits()
    {
        var board = new Board { Name = "Projeto", OwnerId = Guid.NewGuid() };
        var boardRepository = new FakeBoardRepository(new[] { board });
        var unitOfWork = new FakeUnitOfWork();
        var service = new DeleteBoardService(boardRepository, unitOfWork);

        await service.DeleteAsync(board.Id);

        Assert.Empty(boardRepository.SavedItems);
        Assert.Equal(1, unitOfWork.CommitCount);
    }
}

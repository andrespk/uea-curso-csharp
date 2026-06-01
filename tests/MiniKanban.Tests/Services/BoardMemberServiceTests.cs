using MiniKanban.Application.DTOs;
using MiniKanban.Application.Services;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Enums;
using MiniKanban.Exceptions.Users;
using MiniKanban.Tests.Fakes;

namespace MiniKanban.Tests.Services;

public class BoardMemberServiceTests
{
    [Fact]
    public async Task AddAsync_WhenDataIsValid_AddsMemberAndCommits()
    {
        var board = new Board { Name = "Board", OwnerId = Guid.NewGuid() };
        var user = new User { Name = "Ana", Username = "ana", Email = "ana@example.com", PasswordHash = "hash", Role = "User" };
        var memberRepository = new FakeBoardMemberRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new AddBoardMemberService(
            memberRepository,
            new FakeBoardRepository(new[] { board }),
            new FakeUserRepository(new[] { user }),
            unitOfWork);

        var result = await service.AddAsync(new CreateBoardMemberDto
        {
            BoardId = board.Id,
            UserId = user.Id,
            Role = BoardRole.Member
        });

        var member = Assert.Single(memberRepository.SavedItems);
        Assert.Equal(member.Id, result.Id);
        Assert.Equal(BoardRole.Member, result.Role);
        Assert.Equal(1, unitOfWork.CommitCount);
    }

    [Fact]
    public async Task AddAsync_WhenUserIsAlreadyMember_ThrowsBusinessException()
    {
        var boardId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var service = new AddBoardMemberService(
            new FakeBoardMemberRepository(new[] { new BoardMember { BoardId = boardId, UserId = userId } }),
            new FakeBoardRepository(new[] { new Board { Id = boardId, Name = "Board", OwnerId = Guid.NewGuid() } }),
            new FakeUserRepository(new[] { new User { Id = userId, Name = "Ana", Username = "ana", Email = "ana@example.com", PasswordHash = "hash", Role = "User" } }),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.AddAsync(new CreateBoardMemberDto
        {
            BoardId = boardId,
            UserId = userId,
            Role = BoardRole.Member
        }));
    }

    [Fact]
    public async Task AddAsync_WhenRoleIsOwner_ThrowsBusinessException()
    {
        var service = new AddBoardMemberService(
            new FakeBoardMemberRepository(),
            new FakeBoardRepository(),
            new FakeUserRepository(),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.AddAsync(new CreateBoardMemberDto
        {
            BoardId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = BoardRole.Owner
        }));
    }

    [Fact]
    public async Task UpdateAsync_WhenChangingOwnerRole_ThrowsBusinessException()
    {
        var ownerMember = new BoardMember { BoardId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = BoardRole.Owner };
        var service = new UpdateBoardMemberService(
            new FakeBoardMemberRepository(new[] { ownerMember }),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.UpdateAsync(ownerMember.Id, new UpdateBoardMemberDto
        {
            Role = BoardRole.Admin
        }));
    }

    [Fact]
    public async Task RemoveAsync_WhenMemberIsOwner_ThrowsBusinessException()
    {
        var ownerMember = new BoardMember { BoardId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = BoardRole.Owner };
        var service = new RemoveBoardMemberService(
            new FakeBoardMemberRepository(new[] { ownerMember }),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.RemoveAsync(ownerMember.Id));
    }
}

using MiniKanban.Application.DTOs;
using MiniKanban.Application.Services.KanbanColumn;
using MiniKanban.Domain.Entities;
using MiniKanban.Exceptions;
using MiniKanban.Tests.Fakes;

namespace MiniKanban.Tests.Services;

public class KanbanColumnServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenDataIsValid_CreatesColumnAndCommits()
    {
        var board = new Board { Name = "Board", OwnerId = Guid.NewGuid() };
        var columnRepository = new FakeKanbanColumnRepository();
        var unitOfWork = new FakeUnitOfWork();
        var service = new CreateKanbanColumnService(
            columnRepository,
            new FakeBoardRepository(new[] { board }),
            unitOfWork);

        var result = await service.CreateAsync(new CreateKanbanColumnDto
        {
            BoardId = board.Id,
            Name = "To Do",
            Order = 0,
            WipLimit = 5
        });

        var column = Assert.Single(columnRepository.SavedItems);
        Assert.Equal(column.Id, result.Id);
        Assert.Equal("To Do", result.Name);
        Assert.Equal(5, result.WipLimit);
        Assert.Equal(1, unitOfWork.CommitCount);
    }

    [Fact]
    public async Task CreateAsync_WhenOrderAlreadyExists_ThrowsBusinessException()
    {
        var boardId = Guid.NewGuid();
        var service = new CreateKanbanColumnService(
            new FakeKanbanColumnRepository(new[] { new KanbanColumn { BoardId = boardId, Name = "To Do", Order = 0 } }),
            new FakeBoardRepository(new[] { new Board { Id = boardId, Name = "Board", OwnerId = Guid.NewGuid() } }),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.CreateAsync(new CreateKanbanColumnDto
        {
            BoardId = boardId,
            Name = "Doing",
            Order = 0
        }));
    }

    [Fact]
    public async Task CreateAsync_WhenBoardDoesNotExist_ThrowsBusinessException()
    {
        var service = new CreateKanbanColumnService(
            new FakeKanbanColumnRepository(),
            new FakeBoardRepository(),
            new FakeUnitOfWork());

        await Assert.ThrowsAsync<BusinessException>(() => service.CreateAsync(new CreateKanbanColumnDto
        {
            BoardId = Guid.NewGuid(),
            Name = "To Do",
            Order = 0
        }));
    }

    [Fact]
    public async Task UpdateAsync_WhenColumnExists_UpdatesColumnAndCommits()
    {
        var column = new KanbanColumn { BoardId = Guid.NewGuid(), Name = "To Do", Order = 0, WipLimit = 3 };
        var columnRepository = new FakeKanbanColumnRepository(new[] { column });
        var unitOfWork = new FakeUnitOfWork();
        var service = new UpdateKanbanColumnService(columnRepository, unitOfWork);

        var result = await service.UpdateAsync(column.Id, new UpdateKanbanColumnDto
        {
            Name = "Doing",
            Order = 1,
            WipLimit = 4
        });

        Assert.Equal("Doing", result.Name);
        Assert.Equal(1, result.Order);
        Assert.Equal(4, result.WipLimit);
        Assert.NotNull(column.UpdatedAt);
        Assert.Equal(1, unitOfWork.CommitCount);
    }
}
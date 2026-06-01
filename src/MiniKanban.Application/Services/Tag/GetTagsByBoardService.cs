using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Tag;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Tag;

public class GetTagsByBoardService : IGetTagsByBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly ITagRepository _tagRepository;

    public GetTagsByBoardService(
        ITagRepository tagRepository,
        IBoardRepository boardRepository)
    {
        _tagRepository = tagRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<TagResponseDto>> GetByBoardIdAsync(Guid boardId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(boardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        var tags = await _tagRepository.GetByBoardIdAsync(boardId, cancellationToken);
        return tags.Select(t => TagMapping.ToResponse(t));
    }
}
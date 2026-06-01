using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class GetTagsByBoardService : IGetTagsByBoardService, ScopedInjection
{
    private readonly ITagRepository _tagRepository;
    private readonly IBoardRepository _boardRepository;

    public GetTagsByBoardService(
        ITagRepository tagRepository,
        IBoardRepository boardRepository)
    {
        _tagRepository = tagRepository;
        _boardRepository = boardRepository;
    }

    public async Task<IEnumerable<TagResponseDto>> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(boardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        var tags = await _tagRepository.GetByBoardIdAsync(boardId, cancellationToken);
        return tags.Select(t => TagMapping.ToResponse(t));
    }
}


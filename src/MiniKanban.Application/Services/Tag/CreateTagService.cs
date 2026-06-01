using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class CreateTagService : ICreateTagService, ScopedInjection
{
    private readonly ITagRepository _tagRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTagService(
        ITagRepository tagRepository,
        IBoardRepository boardRepository,
        IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TagResponseDto> CreateAsync(CreateTagDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        if (await _tagRepository.NameExistsAsync(request.BoardId, request.Name, cancellationToken))
            throw new BusinessException("Tag name already exists in this board.");

        var tag = TagMapping.ToEntity(request);
        await _tagRepository.AddAsync(tag, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return TagMapping.ToResponse(tag);
    }
}


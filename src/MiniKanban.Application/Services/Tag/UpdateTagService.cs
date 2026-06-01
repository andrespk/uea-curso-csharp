using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateTagService : IUpdateTagService, ScopedInjection
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTagService(
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TagResponseDto> UpdateAsync(Guid id, UpdateTagDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tag = await _tagRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Tag not found.");

        // If name is being updated, check for uniqueness
        if (!string.IsNullOrEmpty(request.Name) && request.Name != tag.Name)
        {
            if (await _tagRepository.NameExistsAsync(tag.BoardId, request.Name, cancellationToken))
                throw new BusinessException("Tag name already exists in this board.");
        }

        var updatedTag = TagMapping.ToEntity(request, tag);
        await _tagRepository.UpdateAsync(updatedTag, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return TagMapping.ToResponse(updatedTag);
    }
}


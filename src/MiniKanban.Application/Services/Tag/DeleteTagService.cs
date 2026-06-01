using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class DeleteTagService : IDeleteTagService, ScopedInjection
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTagService(
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tag = await _tagRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Tag not found.");

        await _tagRepository.DeleteAsync(tag, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}


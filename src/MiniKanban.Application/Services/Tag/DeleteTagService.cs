using MiniKanban.Application.Interfaces.Tag;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Tag;

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
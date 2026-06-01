using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class DeleteKanbanColumnService : IDeleteKanbanColumnService, ScopedInjection
{
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteKanbanColumnService(IKanbanColumnRepository kanbanColumnRepository, IUnitOfWork unitOfWork)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var column = await _kanbanColumnRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Kanban column not found.");

        cancellationToken.ThrowIfCancellationRequested();

        await _kanbanColumnRepository.DeleteAsync(column, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

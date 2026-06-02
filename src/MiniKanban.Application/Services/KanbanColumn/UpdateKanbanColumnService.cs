using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.KanbanColumn;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.KanbanColumn;

public class UpdateKanbanColumnService : IUpdateKanbanColumnService, ScopedInjection
{
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateKanbanColumnService(IKanbanColumnRepository kanbanColumnRepository, IUnitOfWork unitOfWork)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<KanbanColumnResponseDto> UpdateAsync(Guid id, UpdateKanbanColumnDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var column = await _kanbanColumnRepository.GetByIdAsync(id, cancellationToken)
                     ?? throw new BusinessException("Kanban column not found.");

        if (request.Order.HasValue && request.Order.Value < 0)
            throw new BusinessException("Column order cannot be negative.");

        if (request.WipLimit.HasValue && request.WipLimit.Value < 0)
            throw new BusinessException("WIP limit cannot be negative.");

        cancellationToken.ThrowIfCancellationRequested();

        var originalName = column.Name;
        KanbanColumnMapping.ToEntity(request, column);
        if (string.IsNullOrWhiteSpace(column.Name)) column.Name = originalName;

        column.UpdatedAt = DateTime.UtcNow;

        await _kanbanColumnRepository.UpdateAsync(column, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return KanbanColumnMapping.ToResponse(column);
    }
}
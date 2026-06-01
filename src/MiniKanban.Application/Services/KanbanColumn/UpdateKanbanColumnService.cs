using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateKanbanColumnService : IUpdateKanbanColumnService, ScopedInjection
{
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateKanbanColumnService(IKanbanColumnRepository kanbanColumnRepository, IUnitOfWork unitOfWork)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<KanbanColumnResponseDto> UpdateAsync(Guid id, UpdateKanbanColumnDto request)
    {
        var column = await _kanbanColumnRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Kanban column not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))
            column.Name = request.Name;

        if (request.Order.HasValue)
        {
            if (request.Order.Value < 0)
                throw new BusinessException("Column order cannot be negative.");

            column.Order = request.Order.Value;
        }

        if (request.WipLimit.HasValue)
        {
            if (request.WipLimit.Value < 0)
                throw new BusinessException("WIP limit cannot be negative.");

            column.WipLimit = request.WipLimit;
        }

        column.UpdatedAt = DateTime.UtcNow;

        await _kanbanColumnRepository.UpdateAsync(column);
        await _unitOfWork.CommitAsync();

        return KanbanColumnMapping.ToResponse(column);
    }
}

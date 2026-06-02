using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.KanbanColumn;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.KanbanColumn;

public class CreateKanbanColumnService : ICreateKanbanColumnService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateKanbanColumnService(
        IKanbanColumnRepository kanbanColumnRepository,
        IBoardRepository boardRepository,
        IUnitOfWork unitOfWork)
    {
        _kanbanColumnRepository = kanbanColumnRepository;
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<KanbanColumnResponseDto> CreateAsync(CreateKanbanColumnDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request.Order < 0)
            throw new BusinessException("Column order cannot be negative.");

        if (request.WipLimit is < 0)
            throw new BusinessException("WIP limit cannot be negative.");

        if (await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken) == null)
            throw new BusinessException("Board not found.");

        if (await _kanbanColumnRepository.OrderExistsAsync(request.BoardId, request.Order, cancellationToken))
            throw new BusinessException("Column order already exists for this board.");

        cancellationToken.ThrowIfCancellationRequested();

        var column = KanbanColumnMapping.ToEntity(request);

        await _kanbanColumnRepository.AddAsync(column, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return KanbanColumnMapping.ToResponse(column);
    }
}
using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class CreateKanbanColumnService : ICreateKanbanColumnService, ScopedInjection
{
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IBoardRepository _boardRepository;
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

    public async Task<KanbanColumnResponseDto> CreateAsync(CreateKanbanColumnDto request)
    {
        if (request.Order < 0)
            throw new BusinessException("Column order cannot be negative.");

        if (request.WipLimit is < 0)
            throw new BusinessException("WIP limit cannot be negative.");

        if (await _boardRepository.GetByIdAsync(request.BoardId) == null)
            throw new BusinessException("Board not found.");

        if (await _kanbanColumnRepository.OrderExistsAsync(request.BoardId, request.Order))
            throw new BusinessException("Column order already exists for this board.");

        var column = new KanbanColumn
        {
            BoardId = request.BoardId,
            Name = request.Name,
            Order = request.Order,
            WipLimit = request.WipLimit
        };

        await _kanbanColumnRepository.AddAsync(column);
        await _unitOfWork.CommitAsync();

        return KanbanColumnMapping.ToResponse(column);
    }
}

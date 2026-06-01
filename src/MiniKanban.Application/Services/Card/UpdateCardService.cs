using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateCardService : IUpdateCardService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCardService(
        ICardRepository cardRepository,
        IKanbanColumnRepository kanbanColumnRepository,
        IBoardMemberRepository boardMemberRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _cardRepository = cardRepository;
        _kanbanColumnRepository = kanbanColumnRepository;
        _boardMemberRepository = boardMemberRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CardResponseDto> UpdateAsync(Guid id, UpdateCardDto request)
    {
        var card = await _cardRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Card not found.");

        var column = await _kanbanColumnRepository.GetByIdAsync(request.ColumnId ?? card.ColumnId)
            ?? throw new BusinessException("Kanban column not found.");

        if (request.ColumnId.HasValue && request.ColumnId.Value != card.ColumnId && column.WipLimit.HasValue)
        {
            var cardsInColumn = await _cardRepository.CountByColumnIdAsync(column.Id);
            if (cardsInColumn >= column.WipLimit.Value)
                throw new BusinessException("Column WIP limit reached.");
        }

        if (request.AssignedToUserId.HasValue)
        {
            if (await _userRepository.GetByIdAsync(request.AssignedToUserId.Value) == null)
                throw new BusinessException("Assigned user not found.");

            if (!await _boardMemberRepository.ExistsAsync(column.BoardId, request.AssignedToUserId.Value))
                throw new BusinessException("Assigned user is not a board member.");
        }

        if (request.ColumnId.HasValue)
            card.ColumnId = request.ColumnId.Value;

        if (request.AssignedToUserId.HasValue)
            card.AssignedToUserId = request.AssignedToUserId;

        if (!string.IsNullOrWhiteSpace(request.Title))
            card.Title = request.Title;

        if (request.Description != null)
            card.Description = request.Description;

        if (request.Priority.HasValue)
            card.Priority = request.Priority.Value;

        if (request.DueDate.HasValue)
            card.DueDate = request.DueDate;

        card.UpdatedAt = DateTime.UtcNow;

        await _cardRepository.UpdateAsync(card);
        await _unitOfWork.CommitAsync();

        return CardMapping.ToResponse(card);
    }
}

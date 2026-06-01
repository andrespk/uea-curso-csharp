using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class CreateCardService : ICreateCardService, ScopedInjection
{
    private readonly ICardRepository _cardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCardService(
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

    public async Task<CardResponseDto> CreateAsync(CreateCardDto request)
    {
        var column = await _kanbanColumnRepository.GetByIdAsync(request.ColumnId)
            ?? throw new BusinessException("Kanban column not found.");

        if (await _userRepository.GetByIdAsync(request.CreatedByUserId) == null)
            throw new BusinessException("Creator user not found.");

        if (!await _boardMemberRepository.ExistsAsync(column.BoardId, request.CreatedByUserId))
            throw new BusinessException("Creator user is not a board member.");

        if (request.AssignedToUserId.HasValue)
        {
            if (await _userRepository.GetByIdAsync(request.AssignedToUserId.Value) == null)
                throw new BusinessException("Assigned user not found.");

            if (!await _boardMemberRepository.ExistsAsync(column.BoardId, request.AssignedToUserId.Value))
                throw new BusinessException("Assigned user is not a board member.");
        }

        if (column.WipLimit.HasValue)
        {
            var cardsInColumn = await _cardRepository.CountByColumnIdAsync(column.Id);
            if (cardsInColumn >= column.WipLimit.Value)
                throw new BusinessException("Column WIP limit reached.");
        }

        var card = new Card
        {
            ColumnId = request.ColumnId,
            CreatedByUserId = request.CreatedByUserId,
            AssignedToUserId = request.AssignedToUserId,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate
        };

        await _cardRepository.AddAsync(card);
        await _unitOfWork.CommitAsync();

        return CardMapping.ToResponse(card);
    }
}

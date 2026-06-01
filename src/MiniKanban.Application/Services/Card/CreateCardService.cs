using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Card;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Card;

public class CreateCardService : ICreateCardService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly ICardRepository _cardRepository;
    private readonly IKanbanColumnRepository _kanbanColumnRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

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

    public async Task<CardResponseDto> CreateAsync(CreateCardDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var column = await _kanbanColumnRepository.GetByIdAsync(request.ColumnId, cancellationToken)
                     ?? throw new BusinessException("Kanban column not found.");

        if (await _userRepository.GetByIdAsync(request.CreatedByUserId, cancellationToken) == null)
            throw new BusinessException("Creator user not found.");

        if (!await _boardMemberRepository.ExistsAsync(column.BoardId, request.CreatedByUserId, cancellationToken))
            throw new BusinessException("Creator user is not a board member.");

        if (request.AssignedToUserId.HasValue)
        {
            if (await _userRepository.GetByIdAsync(request.AssignedToUserId.Value, cancellationToken) == null)
                throw new BusinessException("Assigned user not found.");

            if (!await _boardMemberRepository.ExistsAsync(column.BoardId, request.AssignedToUserId.Value,
                    cancellationToken))
                throw new BusinessException("Assigned user is not a board member.");
        }

        if (column.WipLimit.HasValue)
        {
            var cardsInColumn = await _cardRepository.CountByColumnIdAsync(column.Id, cancellationToken);
            if (cardsInColumn >= column.WipLimit.Value)
                throw new BusinessException("Column WIP limit reached.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        var card = CardMapping.ToEntity(request);

        await _cardRepository.AddAsync(card, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CardMapping.ToResponse(card);
    }
}
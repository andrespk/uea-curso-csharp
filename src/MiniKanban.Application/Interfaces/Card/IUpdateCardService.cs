using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Card;

public interface IUpdateCardService
{
    Task<CardResponseDto> UpdateAsync(Guid id, UpdateCardDto request, CancellationToken cancellationToken = default);
}
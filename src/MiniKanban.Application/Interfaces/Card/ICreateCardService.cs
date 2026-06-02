using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Card;

public interface ICreateCardService
{
    Task<CardResponseDto> CreateAsync(CreateCardDto request, CancellationToken cancellationToken = default);
}
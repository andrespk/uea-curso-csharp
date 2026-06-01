using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.CardTag;

public interface IAddCardTagService
{
    Task<CardTagResponseDto> AddAsync(CreateCardTagDto request, CancellationToken cancellationToken = default);
}
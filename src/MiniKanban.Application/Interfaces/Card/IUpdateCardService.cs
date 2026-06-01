using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateCardService
{
    Task<CardResponseDto> UpdateAsync(Guid id, UpdateCardDto request);
}

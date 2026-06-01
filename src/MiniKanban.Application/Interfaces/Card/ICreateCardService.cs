using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ICreateCardService
{
    Task<CardResponseDto> CreateAsync(CreateCardDto request);
}

using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ICreateKanbanColumnService
{
    Task<KanbanColumnResponseDto> CreateAsync(CreateKanbanColumnDto request);
}

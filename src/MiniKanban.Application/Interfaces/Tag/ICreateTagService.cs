using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Tag;

public interface ICreateTagService
{
    Task<TagResponseDto> CreateAsync(CreateTagDto request, CancellationToken cancellationToken = default);
}
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces.Tag;

public interface IUpdateTagService
{
    Task<TagResponseDto> UpdateAsync(Guid id, UpdateTagDto request, CancellationToken cancellationToken = default);
}
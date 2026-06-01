using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IUpdateTagService
{
    Task<TagResponseDto> UpdateAsync(Guid id, UpdateTagDto request, CancellationToken cancellationToken = default);
}


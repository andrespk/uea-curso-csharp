using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface ICreateTagService
{
    Task<TagResponseDto> CreateAsync(CreateTagDto request, CancellationToken cancellationToken = default);
}


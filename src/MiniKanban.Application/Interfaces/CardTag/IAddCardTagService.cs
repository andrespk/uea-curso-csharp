using System.Threading;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Interfaces;

public interface IAddCardTagService
{
    Task<CardTagResponseDto> AddAsync(CreateCardTagDto request, CancellationToken cancellationToken = default);
}


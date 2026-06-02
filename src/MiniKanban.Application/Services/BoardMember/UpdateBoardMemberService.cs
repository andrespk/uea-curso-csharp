using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.BoardMember;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.BoardMember;

public class UpdateBoardMemberService : IUpdateBoardMemberService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardMemberService(IBoardMemberRepository boardMemberRepository, IUnitOfWork unitOfWork)
    {
        _boardMemberRepository = boardMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BoardMemberResponseDto> UpdateAsync(Guid id, UpdateBoardMemberDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var member = await _boardMemberRepository.GetByIdAsync(id, cancellationToken)
                     ?? throw new BusinessException("Board member not found.");

        if (member.Role == BoardRole.Owner && request.Role != BoardRole.Owner)
            throw new BusinessException("Board owner role cannot be changed here.");

        if (request.Role == BoardRole.Owner && member.Role != BoardRole.Owner)
            throw new BusinessException("A new owner cannot be assigned here.");

        cancellationToken.ThrowIfCancellationRequested();

        BoardMemberMapping.ToEntity(request, member);
        member.UpdatedAt = DateTime.UtcNow;

        await _boardMemberRepository.UpdateAsync(member, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BoardMemberMapping.ToResponse(member);
    }
}
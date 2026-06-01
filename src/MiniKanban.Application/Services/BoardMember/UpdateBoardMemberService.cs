using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class UpdateBoardMemberService : IUpdateBoardMemberService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardMemberService(IBoardMemberRepository boardMemberRepository, IUnitOfWork unitOfWork)
    {
        _boardMemberRepository = boardMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BoardMemberResponseDto> UpdateAsync(Guid id, UpdateBoardMemberDto request)
    {
        var member = await _boardMemberRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Board member not found.");

        if (member.Role == BoardRole.Owner && request.Role != BoardRole.Owner)
            throw new BusinessException("Board owner role cannot be changed here.");

        if (request.Role == BoardRole.Owner && member.Role != BoardRole.Owner)
            throw new BusinessException("A new owner cannot be assigned here.");

        member.Role = request.Role;
        member.UpdatedAt = DateTime.UtcNow;

        await _boardMemberRepository.UpdateAsync(member);
        await _unitOfWork.CommitAsync();

        return BoardMemberMapping.ToResponse(member);
    }
}

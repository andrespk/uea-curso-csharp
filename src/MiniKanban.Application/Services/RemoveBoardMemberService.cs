using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class RemoveBoardMemberService : IRemoveBoardMemberService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBoardMemberService(IBoardMemberRepository boardMemberRepository, IUnitOfWork unitOfWork)
    {
        _boardMemberRepository = boardMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveAsync(Guid id)
    {
        var member = await _boardMemberRepository.GetByIdAsync(id)
            ?? throw new BusinessException("Board member not found.");

        if (member.Role == BoardRole.Owner)
            throw new BusinessException("Board owner cannot be removed from members.");

        await _boardMemberRepository.DeleteAsync(member);
        await _unitOfWork.CommitAsync();
    }
}

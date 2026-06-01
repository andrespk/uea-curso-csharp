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

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var member = await _boardMemberRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessException("Board member not found.");

        if (member.Role == BoardRole.Owner)
            throw new BusinessException("Board owner cannot be removed from members.");

        cancellationToken.ThrowIfCancellationRequested();

        await _boardMemberRepository.DeleteAsync(member, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

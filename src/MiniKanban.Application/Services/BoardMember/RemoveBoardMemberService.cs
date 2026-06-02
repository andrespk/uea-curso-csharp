using MiniKanban.Application.Interfaces.BoardMember;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.BoardMember;

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
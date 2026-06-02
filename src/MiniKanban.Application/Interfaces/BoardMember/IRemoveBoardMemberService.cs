namespace MiniKanban.Application.Interfaces.BoardMember;

public interface IRemoveBoardMemberService
{
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}
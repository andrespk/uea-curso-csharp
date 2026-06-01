namespace MiniKanban.Application.Interfaces;

public interface IRemoveBoardMemberService
{
    Task RemoveAsync(Guid id);
}

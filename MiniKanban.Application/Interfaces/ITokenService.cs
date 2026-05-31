using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
}

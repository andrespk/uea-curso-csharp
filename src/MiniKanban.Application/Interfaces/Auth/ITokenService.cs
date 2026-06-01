namespace MiniKanban.Application.Interfaces.Auth;

public interface ITokenService
{
    string GenerateToken(Domain.Entities.User user);
    bool ValidateToken(string token);
}
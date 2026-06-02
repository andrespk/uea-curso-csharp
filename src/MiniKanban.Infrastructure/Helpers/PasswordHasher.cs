using System.Security.Cryptography;
using System.Text;

namespace MiniKanban.Infrastructure.Helpers;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var builder = new StringBuilder();
        foreach (var b in bytes) builder.Append(b.ToString("x2"));
        return builder.ToString();
    }

    public static bool Verify(string password, string hashedPassword)
    {
        return Hash(password) == hashedPassword;
    }
}
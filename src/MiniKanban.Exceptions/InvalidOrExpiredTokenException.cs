namespace MiniKanban.Exceptions;

public class InvalidOrExpiredTokenException : Exception
{
    public  InvalidOrExpiredTokenException () : base ("Token de autenticação inválido ou expirado.") { }
}
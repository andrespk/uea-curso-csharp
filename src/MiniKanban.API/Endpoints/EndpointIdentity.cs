using System.Security.Claims;

namespace MiniKanban.API.Endpoints;

internal static class EndpointIdentity
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var userId) ? userId : null;
    }
}

using System.Security.Claims;

namespace TaskManager.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? user.FindFirstValue("sub");

        return Guid.TryParse(sub, out var id)
            ? id
            : throw new InvalidOperationException("El token no contiene un UserId válido.");
    }
}

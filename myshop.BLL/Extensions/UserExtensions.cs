
using System.Security.Claims;

namespace myshop.BLL.Extensions;

public static class UserExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
    public static string GetFullName(this ClaimsPrincipal user)
    {
        return user.FindFirstValue("FullName") ?? string.Empty;
    }
}

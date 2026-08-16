
using System.Security.Claims;

namespace myshop.BLL.Extensions;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public string? Id => user.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? EmailAddress => user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue(ClaimTypes.Name);
        public string? FullName => user.FindFirstValue("FullName");
    }
}

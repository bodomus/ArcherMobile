using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Http;

namespace ArcherMobilApp.Infrastracture.Extensions
{
    public static class IHttpContextAccessorExtension
    {
        public static int CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}

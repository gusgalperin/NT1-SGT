using Domain.Core.Security;
using System;
using System.Linq;
using System.Security.Claims;

namespace Presentation.Web
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Claim(this ClaimsPrincipal user, string type)
        {
            return user.Claims
                .FirstOrDefault(x => x.Type == type)?.Value ?? string.Empty;
        }

        public static string Rol(this ClaimsPrincipal user)
        {
            return user.Claim("Rol");
        }

        public static RolType RolAsEnum(this ClaimsPrincipal user)
        {
            return (RolType) Enum.Parse(typeof(RolType), user.Claim("Rol"));
        }
    }
}
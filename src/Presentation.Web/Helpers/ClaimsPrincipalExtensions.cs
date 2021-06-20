using Domain.Core.Security;
using Domain.Entities;
using System;
using System.Collections.Generic;
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

        public static Rol.Options RolAsEnum(this ClaimsPrincipal user)
        {
            return (Rol.Options) Enum.Parse(typeof(RolType), user.Rol());
        }

        public static IEnumerable<string> Persmisos(this ClaimsPrincipal user)
        {
            return user.Claim("Permisos").Split("|").ToList();
        }

        public static bool Puede(this ClaimsPrincipal user, string permiso)
        {
            return user.Persmisos().Contains(permiso);
        }
    }
}
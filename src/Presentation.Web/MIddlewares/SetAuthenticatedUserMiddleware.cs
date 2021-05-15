using Domain.Core.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Web.MIddlewares
{
    public class SetAuthenticatedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public SetAuthenticatedUserMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAuthenticatedUser user)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                user.UserInfo = new UserInfo
                {
                    Id = Guid.Parse(context.User.Claim("Id")),
                    Nombre = context.User.Claim(ClaimTypes.Name),
                    Rol = context.User.RolAsEnum(),
                };
            }

            await _next(context);
        }
    }
}
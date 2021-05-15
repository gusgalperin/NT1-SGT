using Domain.Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Web.Views.Login
{
    public class AccountController : Controller
    {
        public static string CookieScheme = "AuthCookie";
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, [FromRoute] string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            try
            {
                var user = await _loginService.LoginAsync(model.UserName, model.Password);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim("Rol", user.Rol.ToString()),
                    new Claim("Id", user.Id.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Turnos");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ValidationError", ex.Message);

                return View();
            }
        }

        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
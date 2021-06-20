using Domain.Core.Data.Repositories;
using Domain.Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Web.Views.Login
{
    public class AccountController : Controller
    {
        public static string CookieScheme = "AuthCookie";
        private readonly ILoginService _loginService;
        private readonly IUserRepository _userRepository;

        public AccountController(
            ILoginService loginService,
            IUserRepository userRepository)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var usuarios = (await _userRepository.GetAllAsync())
                .OrderBy(x => x.RolId);

            ViewBag.Usuarios = usuarios;

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
                    new Claim("Permisos", string.Join("|", user.Permisos))
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
                    switch (user.Rol)
                    {
                        case Domain.Entities.Rol.Options.Profesional:
                            return RedirectToAction("Index", "Profesionales");

                        case Domain.Entities.Rol.Options.Recepcionista:
                            return RedirectToAction("Index", "Turnos");

                        default:
                            return RedirectToAction("Index", "Home");
                    }
                    
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
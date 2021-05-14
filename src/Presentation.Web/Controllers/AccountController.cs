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

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, [FromRoute]string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            //return RedirectToAction("Index", "Home");

            if (ValidateLogin(model.UserName, model.Password))
            {
                //var claims = new List<Claim>
                //{
                //    new Claim("user", model.UserName),
                //    new Claim("role", "Member")
                //};

                //await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
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

            return View();
        }

        private bool ValidateLogin(object userName, object password)
        {
            return true;
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

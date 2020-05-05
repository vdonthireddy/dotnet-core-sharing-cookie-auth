using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string returnUri = null)
        {
            if (!string.IsNullOrWhiteSpace(name))// == "Vijay")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, name),
                };
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = returnUri,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                if (returnUri == null)
                    return RedirectToAction("Privacy", "Home");
                else
                    return Redirect(returnUri);
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(_config.GetValue<string>("LoginUrl"));
        }
    }
}

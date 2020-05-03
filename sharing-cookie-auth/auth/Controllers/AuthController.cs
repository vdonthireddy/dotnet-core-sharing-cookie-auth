using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace auth.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string returnUri = null)
        {
            if (name == "Vijay")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "vdonthireddy@gmail.com"),
                    new Claim("FullName", "Vijay Donthireddy"),
                    new Claim(ClaimTypes.Role, "Administrator"),
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
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

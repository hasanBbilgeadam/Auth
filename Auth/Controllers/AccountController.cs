using Auth.Context;
using Auth.Entities;
using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Loginn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel vm)
        {

            var result =  _context.AppUsers.Any(x => x.Email == vm.Email && x.Password == vm.Password);

            if (result)
            {
               
                List<Claim> claims = new List<Claim>()
                { 
                    
                    new Claim(ClaimTypes.Name,vm.Email),
                    new Claim(ClaimTypes.Role,"member"),
                
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                var authProp = new AuthenticationProperties();


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity), authProp);


                TempData["status"] = "Oturum Açma Başarılı";
               return  Redirect("/home/index");

            }



       
            return Redirect("/home/error");
        }

        [Authorize]
        public IActionResult Korunakli()
        {
            return View();
        }

        public async Task<IActionResult>  Logout()
        {
            await  HttpContext.SignOutAsync();
            TempData["status2"] = "çıkış başarılı";
            return Redirect("/home/index");  
        }

        [Authorize]
        public IActionResult Profil()
        {

            var data = _context.AppUsers
                .Where(x => x.Email == User.Identity.Name)
                .FirstOrDefault();

            return View(data);
        }
    }
}

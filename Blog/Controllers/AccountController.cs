using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationContext db;

        public AccountController(ApplicationContext context)

        {

            db = context;
        }

        ///Регистрация
       #region
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            db.Users.Add(user);
            db.SaveChanges();

            return Redirect("/Home");
        }
        #endregion 


        ///Аутентификация
        #region

        [HttpGet]
        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Login(string login, string pass
           )
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Message = "You need to log out first";
                return View();
            }

            if (ModelState.IsValid)
            {
                UserInfo user = await db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == pass);
                if (user != null)
                {
                     await Authenticate(login); // аутентификация

                    return Redirect("/Home");
                }
                ViewBag.Message = "Invalid login or password";
                return View(); ;
            }
            return View();
        }
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
    };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        #endregion

        public IActionResult UserPage()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name;
                ViewBag.name = name;
                return View();
            }
            else
            {
                ViewBag.Message = "You should log in first";
                return View();
            }
        }
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
            ;

        }

    }
}
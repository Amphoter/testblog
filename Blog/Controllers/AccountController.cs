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
using Microsoft.AspNetCore.Authorization;

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
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("UserPage");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            if (User.Identity.IsAuthenticated) { 
            UserInfo checkUser = db.Users.FirstOrDefault(p => p.Login == user.Login);
            if (checkUser == null) {
                db.Users.Add(user);
                db.SaveChanges();
                return Redirect("/Home");
            }
            else
            {
                ViewBag.Message = "This login already exists";
                return View();

            }
            }
            else
            {
                return RedirectToAction("UserPage");
            }



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
                UserInfo user =  db.Users.FirstOrDefault(u => u.Login == login && u.Password == pass);
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

        //User page, editing/deleting user data
        #region
        
        public IActionResult UserPage()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name;
                UserInfo user =  db.Users.FirstOrDefault(p => p.Login == name);
                
                return View(user);
            }
            else
            {
                ViewBag.Message = "You should log in first";
                return View();
            }
        }

        public IActionResult DeleteUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name;
                UserInfo user = db.Users.FirstOrDefault(p => p.Login == name);
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                db.Users.Remove(user);
                db.SaveChanges();
                return Redirect("/Home");

                
            }
            else
            {
                ViewBag.Message = "You should log in first";
                return View();
            }
        }


        [HttpGet]
        public IActionResult EditUser()
        {

            if (User.Identity.IsAuthenticated)
            {
                UserInfo user = db.Users.FirstOrDefault(p => p.Login == User.Identity.Name);
                if (user != null)
                    return View(user);
            }
            return RedirectToAction("Login");
        }
        
        [HttpPost]
        public IActionResult EditUser(int age, string? email, string? name, string? password)
        {
            UserInfo user = db.Users.FirstOrDefault(p => p.Login == User.Identity.Name);
            user.Name = name;
            user.Email = email;
            user.Age = age;
            user.Password = password;
            db.Users.Update(user);
            db.SaveChanges();

            return Redirect("/Home");

            
            ;

        }






        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
            ;

        }


        #endregion
    }
}
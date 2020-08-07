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
                return Redirect("/Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            if (!User.Identity.IsAuthenticated) { 
            UserInfo checkUser = db.Users.FirstOrDefault(p => p.Name == user.Name);
            if (checkUser == null) {
                    UserRole userRole =  db.UserRoles.FirstOrDefault(r => r.Name == "user");
                    if (userRole != null)
                        user.UserRole = userRole;
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
                UserInfo user =  db.Users.Include(u => u.UserRole).FirstOrDefault(u => u.Name == login && u.Password == pass);
                if (user != null)
                {
                     await Authenticate(user); // аутентификация

                    return Redirect("/Home");
                }
                ViewBag.Message = "Invalid login or password";
                return View(); ;
            }
            return View();
        }
        private async Task Authenticate(UserInfo user)
        {
            // создаем один claim
            var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.Name)
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
            {//
                var name = User.Identity.Name;
                UserInfo user =  db.Users.FirstOrDefault(p => p.Name == name);
                
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
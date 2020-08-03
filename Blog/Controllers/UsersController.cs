using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationContext db;

        public UsersController(ApplicationContext context)

        {

            db = context;
        }


        //Вывод пользователей на экран
        #region
        [HttpGet]
        public IActionResult AllUsers()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Users);
            }
            else
            {
                return Redirect("/Account/Login");
            }

        }


        [HttpPost]
        public async Task<IActionResult> OneUser(int? id, string? name)
        {

            if (id != null)
            {
                UserInfo user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            if (id == null)
            {
                UserInfo user = await db.Users.FirstOrDefaultAsync(p => p.Name == name);
                if (user != null)
                    return View(user);
            }

            return NotFound();

        }
        #endregion

    }
}
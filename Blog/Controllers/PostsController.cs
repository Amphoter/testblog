using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationContext db;
        

        public PostsController(ApplicationContext context)

        {

            db = context;
           
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddPost(Post article)
        {
            if (User.Identity.IsAuthenticated)
            {
               // var usrname = User.Identity.Name;
                //UserInfo user = dbuser.Users.FirstOrDefault(p => p.Name == usrname);

                //Post article = new Post();
                article.UserName = User.Identity.Name;
                
                
              db.Posts.Add(article);
                db.SaveChanges();
                return Redirect("/Home");

            }

            ViewBag.Message = "You must be logged in to make a post";
            return View();
        }

        /*
          [HttpPost]
        public IActionResult Create(UserInfo user)
        {
            db.Users.Add(user);
            db.SaveChangesAsync();

            return Redirect("/");
        } 
          
        */

    }
}
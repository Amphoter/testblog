using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
 using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationContext db;
        

        public HomeController(ApplicationContext context)
            
        {
            db = context;
          
        }

       

        ///Privacy,HomePage,Error
        #region
        public IActionResult Index()
        {
            
            return View(db.Posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

    }
}

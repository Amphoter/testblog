using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class ApplicationContext :DbContext
    {

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        public ApplicationContext()
        {
        }
    }
}

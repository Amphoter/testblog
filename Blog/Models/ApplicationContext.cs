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
        public DbSet<UserRole> UserRoles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

           // string adminEmail = "admin@mail.ru";
            //string adminPassword = "123456";

            // добавляем роли
            UserRole adminRole = new UserRole { Id = 1, Name = adminRoleName };
            UserRole userRole = new UserRole { Id = 2, Name = userRoleName };
           // UserInfo adminUser = new UserInfo { Id = 1, Email = adminEmail, Password = adminPassword, UserRoleId = adminRole.Id };

            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { adminRole, userRole });
            modelBuilder.Entity<UserInfo>().HasData(new UserInfo[] {  });
            base.OnModelCreating(modelBuilder);
        }
        public ApplicationContext()
        {
        }
    }
}

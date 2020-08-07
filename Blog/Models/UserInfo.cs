using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int Age { get; set; } // возраст пользователя
        public string Name { get; set; } // имя пользователя
        public string Login { get; set; } // login пользователя
        public string Password { get; set; } // pass пользователя
        public string Email { get; set; } // email пользователя

       public int? UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        
    }
}

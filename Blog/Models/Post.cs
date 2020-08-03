using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        public string MainText { get; set; }
        public string UserName { get; set; }

       /*public Post(int UsrId , string Title, string Text)
        {
            this.Title = Title;
            this.MainText = Text;
            this.UserId = UsrId;

        }
        */

    }
}

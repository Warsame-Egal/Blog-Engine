using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class DisplayBlogPostViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<BlogPost> Blog { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Photo> Photos { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Assignment2.Models
{
    public class Assignment2DataContext : DbContext
    {
        public Assignment2DataContext(DbContextOptions<Assignment2DataContext>
    options)
    : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<BadWord> BadWords { get; set; }


    }
}

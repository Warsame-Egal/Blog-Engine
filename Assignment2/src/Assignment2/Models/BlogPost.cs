using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class BlogPost
    {
        [Key]
        public int BlogPostId
        {
            get;
            set;
        }
        // Foreign key
        [ForeignKey("UserId")]
        public int UserId
        {
            get;
            set;
        }
        User user = new User();


        [StringLength(200)]
        public string Title
        {
            get;
            set;
        }

        [StringLength(400)]
        public string ShortDescription
        {
            get;
            set;
        }

        [StringLength(4000)]
        public string Content
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
        public DateTime Posted
        {
            get;
            set;
        }

        public bool IsAvailable
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Comment
    {
        [Key]
        public int CommentId
        {
            get;
            set;
        }

        // Foreign key
        [ForeignKey("BlogId")]
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

        [Required]
        [StringLength(2048)]
        public string Content
        {
            get;
            set;
        }

        public int Rating
        {
            get;
            set;
        }
    }
}

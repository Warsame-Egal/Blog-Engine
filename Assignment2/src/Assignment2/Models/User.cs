using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class User
    {

        [Key]
        public int UserId //1 for user 2 for admin
        {
            get;
            set;
        }

        // Foreign key
        [ForeignKey("RoleId")]
        public int RoleId
        {
            get;
            set;
        }

        [StringLength(50)]
        public string FirstName
        {
            get;
            set;
        }

        [StringLength(50)]
        public string LastName
        {
            get;
            set;
        }

        [StringLength(50)]
        public string EmailAddress
        {
            get;
            set;
        }


        [StringLength(50)]
        public string Password
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth
        {
            get;
            set;
        }

        [StringLength(50)]
        public string City
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Address
        {
            get;
            set;
        }

        [StringLength(50)]
        public string PostalCode
        {
            get;
            set;
        }

        [StringLength(50)]
        public string Country
        {
            get;
            set;
        }


    }
}
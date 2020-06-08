using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class RegisteredUser
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        public string fullName
        {
            get
            {
                return lastName + ", " + firstName;
            }
        }
        [Display(Name ="Email Address")]
        public string email { get; set; }

        [Display(Name ="Registration Date")]
        public DateTime registrationDate { get; set; }

        [Display(Name = "User's Role")]
        public roles role { get; set; }
        public enum roles
        {
            admin,
            owner,
            renter,
            visitor
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Renter
    {
        public int renterID { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string lastName { get; set; }

        [Display(Name = "Renter's Name")]
        public string fullName
        {
            get
            {
                return lastName + ", " + firstName;
            }
        }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}", ErrorMessage = "Please enter phone numnber in (xxx) xxx-xxxx format")]
        public string phone { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter correct email address")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        public ICollection<Lease> leases { get; set; }
    }
}
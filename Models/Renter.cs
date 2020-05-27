using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Renter
    {
        public int renterID  { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
     public string fullName
        {
            get
            {
                return lastName + ", " + firstName;
            }
        }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
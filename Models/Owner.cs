using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Owner
    {
        public int ownerID  { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public string fullName
        {
            get
            {
                return lastName + ", " + firstName;
            }
        }
        public string email { get; set; }

        public string phone { get; set; }

        public string taxID { get; set; }
        public ICollection<Investor> investor { get; set; }
    }
}
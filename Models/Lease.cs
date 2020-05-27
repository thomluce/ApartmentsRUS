using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Lease
    {
        public int leaseID { get; set; }
        public int renterID { get; set; }
        public virtual Renter renter { get; set; }
        public int  apartmentID { get; set; }
        public virtual Apartment apartment { get; set; }
        public DateTime startDate { get; set; }
        public int duration { get; set; } // in months
        public decimal securityDeposit { get; set; }
        public decimal monthlyRent { get; set; }
        public bool depositRefunded { get; set; }
        public decimal amtRefunded { get; set; }
    }
}
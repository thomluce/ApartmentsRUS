using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Lease
    {
        public int leaseID { get; set; }

        [Display(Name ="Renter")]
        [Required]
        public int renterID { get; set; }

        public virtual Renter renter { get; set; }

        [Display(Name = "Appartment")]
        [Required]
        public int  apartmentID { get; set; }
        public virtual Apartment apartment { get; set; }

        [Display(Name= "Start of lease")]
        [DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)]
        [Required]
        public DateTime startDate { get; set; }

        [Display(Name ="Lease duration in months")]
        [Required]
        public int duration { get; set; }

        [Display(Name = "Security Deposit")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Required]
        public decimal securityDeposit { get; set; }

        [Display(Name ="Montly Rent")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        [Required]
        public decimal monthlyRent { get; set; }

        [Display(Name = "Security Deposit Refunded")]
        public bool depositRefunded { get; set; }

        [Display(Name ="Amount of refund")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal amtRefunded { get; set; }
    }
}
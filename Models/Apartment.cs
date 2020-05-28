using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Apartment
    {
        public int apartmentID { get; set; }

        [Display(Name ="Apartment designation")]
        [Required]
        public string apartment { get; set; }  // Apt 3, Apt C, etc

        [Required]
        public int buildingID { get; set; }
        public virtual Building building  { get; set; }

        [Required]
        public int bedrooms { get; set; }

        [Required]
        public int bathrooms { get; set; }

        [Required]
        public int maxOccupancy { get; set; }
        public ICollection<Lease> leases { get; set; }
    }
}
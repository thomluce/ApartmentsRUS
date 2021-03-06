﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Apartment
    {
        public int apartmentID { get; set; }

        [Display(Name ="Apartment number")]
        [Required]
        public string apartmentNum { get; set; }  // Apt 3, Apt C, etc

        public string apartmentAddr
        {
            get
            {
                return "Apartment " + apartmentNum + " at " + building.buildingAddress;
            }
        }

        [Required]
        public int buildingID { get; set; }
        public virtual Building building  { get; set; }

        [Required]
        [Display(Name = "Num of Bedrooms")]
        public int bedrooms { get; set; }

        [Required]
        [Display(Name = "Num of Bathrooms")]
        public int bathrooms { get; set; }

        [Required]
        [Display(Name = "Max Occupancy")]
        public int maxOccupancy { get; set; }
        public ICollection<Lease> leases { get; set; }

    }
}
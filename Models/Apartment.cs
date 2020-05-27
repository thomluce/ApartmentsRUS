using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Apartment
    {
        public int apartmentID { get; set; }
        public string apartment { get; set; }  // Apt 3, or Apt C, or penthouse
        public int buildingID { get; set; }
        public virtual Building building  { get; set; }
        public int bedrooms { get; set; }
        public int bathrooms { get; set; }
        public int maxOccupancy { get; set; }
    }
}
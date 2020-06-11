using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Building
    {
        public int buildingID { get; set; }

        [Required]
        public string street { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string state { get; set; }

        [Required]
        public string zip { get; set; }

        [Display(Name = "Date of last Inspection")]
        public DateTime? inspectionDate { get; set; }

        [Display(Name = "Inspection Report")]
        public string inspectionResults { get; set; }

        [Display(Name = "Appraised Value")]
        public decimal appraisedValue { get; set; }

        [Display(Name = "Property Tax Rate")]
        [DisplayFormat(DataFormatString ="{0:p3}")]
        public decimal propertyTaxRate { get; set; }

        public string buildingAddress
        {
            get
            {
                return street + " " + city + ", " + state + " " + zip;
            }
        }

        public ICollection<Investor> investors { get; set; }
        public ICollection<Apartment> apartments { get; set; }

        [Display(Name = "Building image")]
        public string buildingImage { get; set; }
    }
}
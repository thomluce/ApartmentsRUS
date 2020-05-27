using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Building
    {
        public int buildingID { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public DateTime? inspectionDate  { get; set; }
        public string inspectionResults { get; set; }
        public decimal appraisedValue { get; set; }

        public decimal propertyTaxRate { get; set; }
        public ICollection<Investor> investors { get; set; }
        public ICollection<Apartment> apartments { get; set; }
    }
}
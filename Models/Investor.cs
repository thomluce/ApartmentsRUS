using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Investor
    {
        [Key]
        public int investorID { get; set; }
        public int ownerID { get; set; }
        public virtual Owner owner{ get; set; }
        public int buildingID { get; set; }
        public virtual Building building { get; set; }
        public DateTime purchaseDate { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal percentOwnership { get; set; }
        public DateTime? saleDate { get; set; }
        public decimal? salePrice { get; set; }
    }
}
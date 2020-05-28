using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class Investor
    {
        public int investorID { get; set; }

        [Required]
        [Display(Name ="Owner")]
        public int ownerID { get; set; }
        public virtual Owner owner{ get; set; }

        [Required]
        [Display(Name = "Building")]
        public int buildingID { get; set; }
        public virtual Building building { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime purchaseDate { get; set; }

        [Required]
        [Display(Name = "Purchaser Price")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal purchasePrice { get; set; }

        [Required]
        [Display(Name = "Percent Ownership")]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal percentOwnership { get; set; }

        [Display(Name = "Sale Price")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public DateTime? saleDate { get; set; }

        [Display(Name = "Sale Price")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public decimal? salePrice { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class BulkUpload
    {
        public int ID { get; set; }
        public string fileName { get; set; }
        public string imageUrl { get; set; }
        public DateTime dateUploaded { get; set; }

        [Display(Name ="Display in slide show?")]
        public bool include { get; set; }
    }
}
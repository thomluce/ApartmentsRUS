using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApartmentsRUS.Models
{
    public class FileModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse Files")]
        public HttpPostedFileBase[] files { get; set; }

    }
}
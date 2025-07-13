using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class FeeType
    {
        public int FeeTypeID { get; set; }

        [Required(ErrorMessage = "Fee Name is required.")]
        public string FeeName { get; set; }
    }
}
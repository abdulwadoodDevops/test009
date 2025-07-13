using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Designation
    {
        public int DesignationID { get; set; }

        [Required(ErrorMessage = "Designation Name is required.")]
        public string DesignationName { get; set; }
    }
}
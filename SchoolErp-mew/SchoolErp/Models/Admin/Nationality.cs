using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Nationality
    {
        public int CountryID { get; set; }       
                
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Country Name is required.")]
        public string NationalityName { get; set; }

    }
}
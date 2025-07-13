using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class City
    {
        public int CityID { get; set; }

        public string CityCode { get; set; }

        [Required(ErrorMessage = "City Name is required.")]
        public string CityName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class AssignClass
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public string Staffname { get; set; }

        [Required(ErrorMessage = "Class Level Name is required.")]
        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Class Level Name is required.")]
        public string ClassLevelName { get; set; }

        [Required(ErrorMessage = "Prefix is required.")]
        public string Prefix { get; set; }
    }
}
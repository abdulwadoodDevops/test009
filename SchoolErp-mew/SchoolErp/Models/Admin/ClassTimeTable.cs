using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class ClassTimeTable
    {
        public int ID { get; set; }

        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Class Level Name is required.")]
        public string ClassLevelName { get; set; }

        [Required(ErrorMessage = "Days is required.")]
        public string Days { get; set; }

        [Required(ErrorMessage = "Period1 is required.")]
        public string Period1 { get; set; }

        [Required(ErrorMessage = "Period2 is required.")]
        public string Period2 { get; set; }

        [Required(ErrorMessage = "Period3 is required.")]
        public string Period3 { get; set; }

        [Required(ErrorMessage = "Period4 is required.")]
        public string Period4 { get; set; }

        [Required(ErrorMessage = "Period5 is required.")]
        public string Period5 { get; set; }

        [Required(ErrorMessage = "Period6 is required.")]
        public string Period6 { get; set; }

     

    }
}
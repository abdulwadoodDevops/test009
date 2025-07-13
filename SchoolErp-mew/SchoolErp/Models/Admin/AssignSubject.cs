using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class AssignSubject
    {
        public int ID { get; set; }

        public int StaffID { get; set; }

        [Required(ErrorMessage = "staff Name is required.")]
        public string staffname { get; set; }

        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Class Level Name is required.")]
        public string ClassLevelname { get; set; }

        [Required(ErrorMessage = "Prefix is required.")]
        public string Prefix { get; set; }

    }
}
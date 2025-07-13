using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Subject
    {
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Subject Name is required.")]
        public string Subjectname { get; set; }

        public int SectionID { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        public string SectionName { get; set; }


 
    }
}
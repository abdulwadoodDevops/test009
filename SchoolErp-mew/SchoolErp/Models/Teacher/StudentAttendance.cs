using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Teacher
{
    public class StudentAttendance
    {
        public int ID;
        [Required(ErrorMessage = "AdmissionNo is required.")]
        public int AdmissionNo { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        public string Studentname { get; set; }  

        [Required(ErrorMessage = "DOB is required.")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Remark is required.")] 
        public string Remark { get; set; }

        
    }
}
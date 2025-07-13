using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Student
    {
        public int AdmissionNo { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        //public DateTime DOB { get; set; }
        public string DOB { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Religion is required.")]
        public string Religion { get; set; }

       
        public int SessionID { get; set; } 

        [Required(ErrorMessage = "Session Name is required.")]
        public string SessionName { get; set; }

        [Required(ErrorMessage = "Term is required.")]
        public string Term { get; set; }

        [Required(ErrorMessage = "Class Level is required.")]
        public string ClassLevel { get; set; }

        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Aphabet is required.")]
        public string ClassAphabet { get; set; }

        [Required(ErrorMessage = "AdmissionDate is required.")]
        public string AdmissionDate { get; set; }

        [Required(ErrorMessage = "Father Name is required.")]
        public string Fathername { get; set; }

        [Required(ErrorMessage = "Mother Name is required.")]
        public string Mothername { get; set; }

        [Required(ErrorMessage = "Father Phone is required.")]
        public string FatherPhone { get; set; }

        [Required(ErrorMessage = "Mother Phone is required.")]
        public string MotherPhone { get; set; }

        [Required(ErrorMessage = "CountryName is required.")]
        public string CountryName { get; set; }

        public DateTime DateTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Teacher
{
    public class SubjectMark
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Term is required.")]
        public string Term { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public int SessionID { get; set; }

        [Required(ErrorMessage = "SessionName is required.")]
        public string SessionName { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Class Levelname is required.")]
        public string ClassLevelname { get; set; }

        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Subject Name is required.")]
        public string Subjectname { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Admission No is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int AdmissionNo { get; set; }

        [Required(ErrorMessage = "FirstCA is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int FirstCA { get; set; }

        [Required(ErrorMessage = "SecondCA is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int SecondCA { get; set; }

        [Required(ErrorMessage = "ThirdCA is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int ThirdCA { get; set; }

        [Required(ErrorMessage = "Exams is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int Exams { get; set; }

        public int Total { get; set; }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Staff
    {
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryID { get; set; }

        public int CityID { get; set; }

        [Required(ErrorMessage = "City Name is required.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "StaffType")]
        public string StaffType { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public string Staffname { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public int DesignationID { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Phone1 is required.")]
        public string Phone1 { get; set; }

        [Required(ErrorMessage = "Phone2 is required.")]
        public string phone2 { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Staff Address is required.")]
        public string StaffAddress { get; set; }

        [Required(ErrorMessage = "DateOfAppointment is required.")]
        public string DateOfAppointment { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; }
               

        

        [Required(ErrorMessage = "YOE is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public int YOE { get; set; }

        [Required(ErrorMessage = "Previous Org is required.")]
        public string previouseOrg { get; set; }

        public string Resume { get; set; }

        [Required(ErrorMessage = "Account Status is required.")]
        public string AccountStatus { get; set; }

        public HttpPostedFileBase ResumeImageFile { get; set; } 
        
        public DateTime DateTime { get; set; }

    }
}
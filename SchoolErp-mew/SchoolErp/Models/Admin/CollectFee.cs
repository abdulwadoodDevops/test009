using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class CollectFee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "AdmissionNo is required.")]
        public int AdmissionNo { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        public string StudentName { get; set; }

        public int ClassLevelID { get; set; }

        [Required(ErrorMessage = "Class Level Name is required.")]
        public string ClassLevelname { get; set; }

        public int SessionID { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public string Session { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "TellerNo is required.")]
        public string TellerNo { get; set; }

        [Required(ErrorMessage = "Bank is required.")]
        public string Bank { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        
    }


}
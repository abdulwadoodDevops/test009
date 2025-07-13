using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class StaffAttendance
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public string Staffname { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Remark is required.")]
        public string Remark { get; set; }
    }
}
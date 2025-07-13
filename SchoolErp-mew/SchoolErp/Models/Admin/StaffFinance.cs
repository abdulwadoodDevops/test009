using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class StaffFinance
    {
        public int ID { get; set; }

        public int staffID { get; set; }

        [Required(ErrorMessage = "Staff Name is required.")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "Basic Salary is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "House Allowance is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]    

        public decimal HouseAllowance { get; set; }

        [Required(ErrorMessage = "Transport Allowance is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal TransportAllowance { get; set; }

        [Required(ErrorMessage = "Late ComingFee is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal LateComingFee { get; set; }

        [Required(ErrorMessage = "Tax is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal Tax { get; set; }

        [Required(ErrorMessage = "Vat is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal Vat { get; set; }

        public decimal TotalPay { get; set; }
    }
}
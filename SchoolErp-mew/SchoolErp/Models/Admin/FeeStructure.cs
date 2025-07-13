using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class FeeStructure
    {
        public int FeeStructureID { get; set; }

        [Required(ErrorMessage = "FeeType Name is required.")]
        public int FeeTypeID { get; set; }

        [Required(ErrorMessage = "FeeType Name is required.")]
        public string FeeTypename { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        public int SectionID { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        public string SectionName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid integer.")]
        public decimal Amount { get; set; }
    }
}
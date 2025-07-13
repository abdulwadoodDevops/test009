using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class ClassLevel
    {
        public int LevelID { get; set; }

        [Required(ErrorMessage = "Level Name is required.")]
        public string Levelname { get; set; }

        public int SectionID { get; set; }

        [Required(ErrorMessage = "Section is required.")]
        public string Section { get; set; }

    
    }
}
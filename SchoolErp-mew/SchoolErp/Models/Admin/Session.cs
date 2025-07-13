using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolErp.Models.Admin
{
    public class Session
    {
        public int SessionID { get; set; }



        [Required(ErrorMessage = "Session Name is required.")]        
        public string SessionName { get; set; }

        [Required(ErrorMessage = "Term is required.")]
        public string Term { get; set; }

        [Required(ErrorMessage = "Remember is required.")]
        public bool Remember { get; set; }

        [Required(ErrorMessage = "Active Status is required.")]
        public string IsActive { get; set; }
    }
}
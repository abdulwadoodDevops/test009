using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolErp.Models.Admin
{
    public class Role
    {
        public int RoleID { get; set; }

        public string Rolename { get; set; }

        public int StaffID { get; set; }

        public string Staffname { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }


    }
}
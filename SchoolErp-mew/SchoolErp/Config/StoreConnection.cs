using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SchoolErp.Config
{
    public class StoreConnection
    {
        public static string GetConnection()
        {
            return ConfigurationManager.ConnectionStrings["SchoolErpSystem"].ConnectionString;

       }
    }
}
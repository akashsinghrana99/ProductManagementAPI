using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModAdmin
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string oldpassword { get; set; }
        
    }
}
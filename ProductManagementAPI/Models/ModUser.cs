using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public string OldPassword { get; set; }
        public string ConFirm_password { get; set; }
        public int status { get; set; }
        public DateTime create_date { get; set; }
        public DateTime update_date { get; set; }


    }
}
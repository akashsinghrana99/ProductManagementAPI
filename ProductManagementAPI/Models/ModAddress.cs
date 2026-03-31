using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModAddress
    {
        public int id { get; set; }
        public string pincode { get; set; }
        public string lanmark { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string bulding_no { get; set; }
        public int status { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }


    }
}
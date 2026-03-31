using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModUnit
    {
        public int id { get; set; }
        public string unit_name { get; set; }
        public string unit { get; set; }
         public int catid { get; set; }
         public int subcatid { get; set; }
         public int status { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updateDate { get; set; }

    }
}
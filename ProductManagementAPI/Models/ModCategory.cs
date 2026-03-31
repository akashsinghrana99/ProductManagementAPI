using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModCategory
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image {  get; set; }
        public int status {  get; set; }
        public DateTime createdDate {  get; set; }
        public DateTime updatedate { get; set; }

    }
}
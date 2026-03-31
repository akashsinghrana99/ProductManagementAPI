using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModDeliveryBoy
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobile_no { get; set; }
        public string email { get; set; }
        public DateTime? dob { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string pin { get; set; }
        public string identity_proof { get; set; }
        public int status { get; set; }
        public string image { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime updatedDate { get; set; }
    }
}
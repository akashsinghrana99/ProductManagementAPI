using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModSlider
    {
        public int id {  get; set; }
        public int cat_id {  get; set; }
        public string slider_name {  get; set; }
        public string image { get; set; }
        public int status { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updatedate { get; set; }



    }
}
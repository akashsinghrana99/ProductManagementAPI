using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModProduct
    {
        public int id {  get; set; }
        public int catId { get; set; }
        public int subcat_id {  get; set; }
        public string product_name {  get; set; }
        public int unit {  get; set; }
        public string hsn_code { get; set; }
        public string product_code { get;  set; }
        public string quantity { get; set; }
        public string txt_type { get; set; }
        public string no_of_item { get; set; }
        public string price { get; set; }
        public string gst_per { get; set; }
        public string gst_amount { get; set; }
        public string amount_with_gst { get; set; }
        public string sell_price { get;set; }
        public string mrp_price { get;set; }
        public string discount { get;set; }
        public string image { get;set; }
        public string product_description { get;set; }
        public string total_amount { get;set; }
        public int status { get;set; }
        public DateTime createdDate { get;set; }
        public DateTime updateDate { get;set; }

        



    }
}
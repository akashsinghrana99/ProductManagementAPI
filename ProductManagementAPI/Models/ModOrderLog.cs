using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class ModOrderLog
    {
        public int Id { get; set; }
        public string order_id { get; set; }
        public string product_id { get; set; }
        public string quantity { get; set; }
        public string product_name { get; set; }
        public string unit { get; set; }
        public string product_code { get; set; }
        public string txt_type { get; set; }
        public string no_of_item { get; set; }
        public string price { get; set; }
        public string gst_per { get; set; }
        public string gst_amount { get; set; }
        public string sell_price { get; set; }
        public string mrp_prise { get; set; }
        public string discount { get;set; }
        public string total_amount { get; set; }
        public string userid { get; set; }
        public int status { get; set; }
        public DateTime createDate {  get; set; }
        public DateTime updateDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class MordCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal prise { get; set; }
        public string sellprise { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
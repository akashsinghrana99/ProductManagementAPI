using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementAPI.Models
{
    public class CommonResponse
    {
        public string Status {  get; set; }
        public string Message { get; set; }
        public object Payload {  get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class ErrorResponse
    {
        public string TimeStamp { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string OrderId { get; set; }
    }
}
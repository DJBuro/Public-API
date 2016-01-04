using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.WebApiServices.Models
{
    public class GprsPrinterIbacs
    {
        public string Printer_id { get; set; }

        public string Order_id { get; set; }

        public string Status { get; set; }

        public string Msg { get; set; }

        public string Delivery_time { get; set; }
    }
}
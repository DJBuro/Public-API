using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Orders.CallbackServices.Models
{
    /// <summary>
    /// IBACS callback model - do not modify
    /// </summary>
    public class GprsPrinterIbacs
    {
        public string Printer_id { get; set; }

        public string Order_id { get; set; }

        public string Status { get; set; }

        public string Msg { get; set; }

        public string Delivery_time { get; set; }
    }
}

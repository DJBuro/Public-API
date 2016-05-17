using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{

    public class OrderSummary 
    {
        public decimal FinalPrice { get; set; }
        public DateTime OrderWantedTime { get; set; }
    }

}
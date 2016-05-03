using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{

    public class StoreOrdersSummary 
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public int ReceivedByStore { get; set; }
        public int InOven { get; set; }
        public int ReadyToDispatch { get; set; }
        public int OutForDelivery { get; set; }
    }

}
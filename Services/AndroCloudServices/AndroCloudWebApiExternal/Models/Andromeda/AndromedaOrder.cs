using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class AndromedaOrder
    {
        public string partnerReference { get; set; }
        public string type { get; set; }
        public string orderTimeType { get; set; }

        private DateTime _orderWantedTime;
        public DateTime orderWantedTime 
        {
            get { return _orderWantedTime; } 
            set 
            {
                this._orderWantedTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        private DateTime _orderPlacedTime;
        public DateTime orderPlacedTime 
        {
            get { return _orderPlacedTime; }
            set 
            {
                this._orderPlacedTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        public int timeToTake { get; set; }
        //public string chefNotes { get; set; }
        public string oneOffDirections { get; set; }
        public int estimatedDeliveryTime { get; set; }
        public Customer customer { get; set; }
        public Pricing pricing { get; set; }
        public List<OrderLine> orderLines { get; set; }
        public List<OrderPayment> orderPayments { get; set; }
    }
}
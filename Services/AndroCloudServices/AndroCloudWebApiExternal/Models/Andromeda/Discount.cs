using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Models.Andromeda
{
    public class Discount
    {
        public string type { get; set; }

        public string discountType { get; set; }

        public int discountTypeAmount { get; set; }

        public int discountAmount { get; set; }

        public string initialDiscountReason { get; set; }
    }
}
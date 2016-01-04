using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class OrderLineTopping
    {
        public string Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
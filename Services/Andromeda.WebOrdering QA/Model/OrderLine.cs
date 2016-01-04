using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class OrderLine
    {
        public string Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Person { get; set; }
        public string Instructions { get; set; }
        public List<OrderLineTopping> AddToppings { get; set; }
        public List<OrderLineTopping> RemoveToppings { get; set; }
    }
}
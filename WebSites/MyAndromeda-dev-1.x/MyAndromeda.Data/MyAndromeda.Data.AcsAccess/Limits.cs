using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.AcsOrders
{
    public class Limit
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class LineItemLimits 
    {
        public Limit AmountOfOrders { get; set; }
        public Limit QuantitySoldInAllOrders { get; set; }
        public Limit SumPriceOfAllItemsInAllOrders { get; set; }
    }
}

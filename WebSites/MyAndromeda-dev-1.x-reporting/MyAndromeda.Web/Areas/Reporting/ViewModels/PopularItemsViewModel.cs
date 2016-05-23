using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class PopularItemsViewModel
    {
        public int OrderQuantityMax { get; set; }
        public decimal OrderSumMax { get; set; }
        public FilterQuery Filter { get; set; }
    }
}
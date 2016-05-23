using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query
{
    public class FilterQuery
    {
        public bool ShowOnlineOrders { get; set; }
        public bool ShowInStoreOrders { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        public DateTime? FilterFrom { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        public DateTime? FilterTo { get; set; }

        public double TotalDays 
        {
            get 
            {
                if (!FilterTo.HasValue && !FilterFrom.HasValue)
                    return 0;

                TimeSpan block = FilterTo.GetValueOrDefault(FilterFrom.GetValueOrDefault()).Subtract(FilterFrom.GetValueOrDefault());

                return block.TotalDays;
            } 
        }
    }
}

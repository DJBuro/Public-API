using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.Api.Data.Models
{
    public class DataWarehouseChain
    {
        public List<DataWareHouseStore> Stores { get; set; }
        public string Name { get; set; }
    }

    public class DataWarehouseOrder
    {
        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        /// <value>The application id.</value>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        public string ExternalSiteId { get; set; }

        /// <summary>
        /// Gets or sets the final price.
        /// </summary>
        /// <value>The final price.</value>
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public string OrderType { get; set; }

        public DateTime? WantedTime { get; set; }
    }

    public class DataWareHouseStore
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get; set; }

        /// <summary>
        /// Gets or sets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        public string ExternalSiteId { get; set; }

        /// <summary>
        /// Gets or sets the name of the external site.
        /// </summary>
        /// <value>The name of the external site.</value>
        public string ExternalSiteName { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public List<DataWarehouseOrder> Orders { get; set; }
        public string Name { get; set; }
    }
}
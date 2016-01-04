using System;
using System.Linq;

namespace MyAndromeda.Services.WebHooks.Models
{
    public class OrderStatusChange : IHook, ISpecificHook
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
        public string ExternalSiteId { get;set; }

        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        //public Guid InternalOrderId { get; set; }

        /// <summary>
        /// Gets or sets the external order id.
        /// </summary>
        /// <value>The external order id.</value>
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Gets or sets the rameses order num.
        /// </summary>
        /// <value>The rameses order num.</value>
        public int? RamesesOrderNum { get; set; }

        /// <summary>
        /// Gets or sets the external acs id.
        /// </summary>
        /// <value>The external acs id.</value>
        public int? AcsApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets the internal order id.
        /// </summary>
        /// <value>The internal order id.</value>
        public Guid? InternalOrderId { get; set; }
    }
}
using System;

namespace MyAndromeda.Services.WebHooks.Models
{
    public class UpdateDeliveryTime : IHook 
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
        /// Gets or sets the edt.
        /// </summary>
        /// <value>The edt.</value>
        public int Edt { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }
    }
}
using System;

namespace MyAndromeda.Services.WebHooks.Models
{
    public class OutgoingWebHookStoreOnlineStatus : IHook
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get; set; }

        public string ExternalSiteId
        {
            get;
            set;
        }

        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the online.
        /// </summary>
        /// <value>The online.</value>
        public bool Online { get; set; }
        //public string ReportedBy { get; set; }
        /// <summary>
        /// Gets or sets who reported the store as online. Hub1, Hub2 etc.
        /// </summary>
        /// <value>The reported by.</value>
    }
}
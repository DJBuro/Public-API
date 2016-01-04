using System;
using System.Collections.Generic;

namespace MyAndromeda.Services.WebHooks.Models
{
    public class MenuChange : IHook 
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
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }
    }

    public class MenuItemsChanged : IHook 
    {
        /// <summary>
        /// Gets or sets the andromeda site id.
        /// </summary>
        /// <value>The andromeda site id.</value>
        public int AndromedaSiteId { get;set; }

        /// <summary>
        /// Gets or sets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        public string ExternalSiteId { get;set; }

        /// <summary>
        /// Gets or sets the source of the update.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get;set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<MenuItemChange> Items { get; set; }
    }

    public class MenuItemChange 
    {
        public int Id {get;set;}
        public string Name {get;set; }
        public string WebName {get;set;}
        public string WebDescription {get;set;}
        public bool Enabled {get;set;}
    }
}
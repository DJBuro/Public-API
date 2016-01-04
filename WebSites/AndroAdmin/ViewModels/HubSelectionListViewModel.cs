using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;
using AndroCloudDataAccess.Domain;

namespace AndroAdmin.ViewModels
{
    public class HubSelectionListViewModel
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string ExternalStoreId { get; set; }

        /// <summary>
        /// Gets or sets the available hubs.
        /// </summary>
        /// <value>The hubs.</value>
        public ICollection<Guid> SelectedHubs { get; set; }
        public ICollection<HubItem> AllHubs { get; set; }
    }

    public class HubSelectionEditViewModel 
    {
        /// <summary>
        /// Gets or sets the store id.
        /// </summary>
        /// <value>The id.</value>
        public int StoreId { get;set; }

        /// <summary>
        /// Gets or sets the external store id.
        /// </summary>
        /// <value>The external store id.</value>
        public string ExternalStoreId { get; set; }

        /// <summary>
        /// Gets or sets the available hubs.
        /// </summary>
        /// <value>The hubs.</value>
        public IEnumerable<HubItem> AllHubs { get; set; }

        /// <summary>
        /// Gets or sets the selected hubs .
        /// </summary>
        /// <value>The selected hubs.</value>
        public IEnumerable<Guid> SelectedHubs { get; set; }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroAdmin.ThreatBoard.ViewModels
{
    public class StoreViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Andromeda Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the create at UTC.
        /// </summary>
        /// <value>The create at UTC.</value>
        public DateTime LastUpdatedAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>The connections.</value>
        public ICollection<StoreConnectionViewModel> Connections { get; set; }
    }

    public static class StoreViewModelExtensions 
    {
        public static StoreViewModel ToViewModel(this Models.Store store) 
        {
            var connections = store.Connections.Select(e=> new StoreConnectionViewModel(){ 
                Connected = e.Value.Connected,
                Host = e.Key
            }).ToList();

            return new StoreViewModel()
            {
                LastUpdatedAtUtc = store.LastUpdateUtc,
                Id = store.AndromedaSiteId,
                Name = store.AndromedaSiteId.ToString(),
                Connections = connections
            };
        }
    }
}

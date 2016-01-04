using System;
using System.Collections.Generic;
using System.Linq;
using AndroAdminDataAccess.Domain;
using System.ComponentModel.DataAnnotations;

namespace AndroAdmin.ViewModels.HostV2
{
    public class HostStoreSelectionViewModel
    {
        public Store Store { get; set; }

        public IEnumerable<AndroAdminDataAccess.EntityFramework.HostV2> StoreHosts { get; set; }
        public IEnumerable<Guid> UserSelectedStoreHosts { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Dictionary<string, List<AndroAdminDataAccess.EntityFramework.HostV2>> PossibleHosts { get; set; }
        public IList<ACSApplication> Applications { get; set; }
    }
}
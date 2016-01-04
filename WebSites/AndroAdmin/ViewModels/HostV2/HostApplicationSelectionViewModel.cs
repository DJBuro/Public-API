using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.ViewModels.HostV2
{
    public class HostApplicationSelectionViewModel 
    {
        public IEnumerable<AndroAdminDataAccess.EntityFramework.HostV2> ApplicationHosts { get; set; }

        public IEnumerable<Guid> UserSelectedAppplicationHosts { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        public Dictionary<string, List<AndroAdminDataAccess.EntityFramework.HostV2>> PossibleHosts { get; set; }

        public ACSApplication Application { get; set; }
    }
}
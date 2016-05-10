using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class StoreEnrolmentViewModel
    {
        public int StoreId { get; set; }
        public SiteDomainModel Site { get; set; }

        public IEnumerable<IEnrolmentLevel> Options { get; set; }
        public int[] SelectedOptionId { get; set; }
        
        public IEnumerable<IPermission> CurrentPermissions { get; set; }
    }
}
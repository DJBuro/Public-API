using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class UpdatePermissisonsViewModel
    {
        public bool CreateRole { get; set; }
        public string Name { get; set; }

        public IEnumerable<IPermission> PossiblePermissions { get; set; }
        public int[] SelectedPermissions { get; set; }
    }
}
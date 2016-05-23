using System;
using System.Linq;
using System.Collections.Generic;

namespace MyAndromeda.Core.Authorization
{

    public class PermissionGroup
    {
        public PermissionGroup() 
        {
            this.StorePermissions = Enumerable.Empty<IPermission>();
            this.UserPermissions = Enumerable.Empty<IPermission>();
        }

        public IEnumerable<IPermission> StorePermissions { get; set; }
        public IEnumerable<IPermission> UserPermissions { get; set; } 



        //chain permissions ? 
    }

}
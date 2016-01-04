using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Core.Authorization
{
    public interface IPermission
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Category { get; set; }

        //PermissionType PermissionType { get; set; }
    }

    public enum PermissionType
    {
        Unknown,
        UserRole,
        StoreEnrolement
    }

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

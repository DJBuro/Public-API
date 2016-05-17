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

}

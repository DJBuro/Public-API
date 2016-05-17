using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.Site;

namespace MyAndromeda.Data.Model.MyAndromeda
{
    public partial class Role : IUserRole
    {
        private IEnumerable<IPermission> effectivePermissions;

        public IEnumerable<IPermission> EffectivePermissions 
        {
            get 
            {
                return this.effectivePermissions ?? (this.RolePermissions.Any() ? this.RolePermissions.Select(e => e.Permission) : Enumerable.Empty<IPermission>());    
            }
            set 
            {
                this.effectivePermissions = value;   
            }
        }
    }
}
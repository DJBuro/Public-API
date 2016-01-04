using System.Collections.Generic;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Authorization.ViewModels
{
    public class UserRoleViewModel : IUserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<IPermission> EffectivePermissions { get; set; }
    }
}
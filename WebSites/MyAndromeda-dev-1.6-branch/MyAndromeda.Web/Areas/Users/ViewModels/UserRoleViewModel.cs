using MyAndromeda.Core.Authorization;
using MyAndromeda.Core.User;
using System;
using System.Linq;

namespace MyAndromeda.Web.Areas.Users.ViewModels
{
    public class UserRoleViewModel
    {
        public MyAndromedaUser User { get; set; }

        public IUserRole[] AvailableRoles { get; set; }

        public int[] SelectedRoles { get; set; }
    }

    public class RoleViewModel 
    {
        public string Name { get; set; }
    }
}
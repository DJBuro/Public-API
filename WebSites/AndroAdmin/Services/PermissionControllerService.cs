using AndroAdmin.DataAccess;
using AndroUsersDataAccess.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Humanizer;

namespace AndroAdmin.Services
{
    public class PermissionControllerService
    {
        private readonly IPermissionDAO permissionsDataAccess = AndroUsersDataAccessFactory.GetPermissionsDAO();

        public void EnsurePermissionsExitForEditingHosts()
        {
            var permissions = new[] 
            { 
                AndroAdmin.Permissions.EditServicesHostList.EditApplicationHostList,
                AndroAdmin.Permissions.EditServicesHostList.EditHostList,
                AndroAdmin.Permissions.EditServicesHostList.EditHostTypeList,
                AndroAdmin.Permissions.EditServicesHostList.EditStoreHostList,
                AndroAdmin.Permissions.EditServicesHostList.ReadApplicationHostList,
                AndroAdmin.Permissions.EditServicesHostList.ReadHostList,
                AndroAdmin.Permissions.EditServicesHostList.ReadHostTypeList,
                AndroAdmin.Permissions.EditServicesHostList.ReadStoreHostList
            };

            var permissionList = permissions.Select(e => new AndroUsersDataAccess.Domain.Permission()
            {
                Name = e,
                Description = e.Humanize(LetterCasing.Sentence)
            });

            permissionsDataAccess.AddPermissions(permissionList);
        }
    }
}
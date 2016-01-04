using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Authorization.Management;
using MyAndromeda.Web.Areas.Authorization.ViewModels;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Core.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.Web.Areas.Authorization.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionManager permissionManager;
        private readonly INotifier notifier;
        private readonly IEnrolmentService enrolmentService;
        private readonly IUserRoleDataService userRoleDataService;
        private readonly IAuthorizer authorizer;

        public PermissionController(IPermissionManager permissionManager, INotifier notifier, IEnrolmentService enrolmentService, IUserRoleDataService userRoleDataService, IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.userRoleDataService = userRoleDataService;
            this.enrolmentService = enrolmentService;
            this.permissionManager = permissionManager;
            this.notifier = notifier;
        }

        public ActionResult AssignPermissionsToRole(string name) 
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
                return new HttpUnauthorizedResult();

            var role = this.userRoleDataService.Get(name);
            var rolePermissions = this.permissionManager.GetEffectivePermissionsForRole(role);

            var viewModel = new UpdatePermissisonsViewModel();
            
            viewModel.Name = name;
            viewModel.PossiblePermissions = this.permissionManager.GetInternalPermissions(permissionType: PermissionType.UserRole);
            viewModel.SelectedPermissions = rolePermissions.Select(e => e.Id).ToArray();

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("AssignPermissionsToRole")]
        public ActionResult AssignPermissionsToRolePost(UpdatePermissisonsViewModel viewModel)
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
                return new HttpUnauthorizedResult();

            var role = this.userRoleDataService.Get(viewModel.Name);
            //var rolePermissions = this.permissionManager.GetEffectivePermissionsForRole(role);

            var allPermissions = this.permissionManager.GetInternalPermissions(permissionType: PermissionType.UserRole);
            var relatedPermissions = 
                viewModel.SelectedPermissions == null || viewModel.SelectedPermissions.Length == 0 ?
                allPermissions = Enumerable.Empty<IPermission>() :
                allPermissions.Where(e => viewModel.SelectedPermissions.Contains(e.Id));

            if (!this.ModelState.IsValid) 
            {
                viewModel.PossiblePermissions = allPermissions;
                return this.View(viewModel);
            }

            this.permissionManager.UpdatePermissions(role, relatedPermissions);
            this.notifier.Notify(string.Format("Permissions have been updated for {0}", role.Name));

            return RedirectToAction("Levels", "UserRoleAuthorization");
        }

        public ActionResult AssignPermissionsToRoleStoreEnrolment(string name)
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
                return new HttpUnauthorizedResult();

            var enrolmentLevel = this.enrolmentService.GetEnrolmentLevel(name);
            var rolePermissions = this.permissionManager.GetEffectivePermissionsForEnrolment(enrolmentLevel);

            var viewModel = new UpdatePermissisonsViewModel();

            viewModel.Name = name;
            viewModel.PossiblePermissions = this.permissionManager.GetInternalPermissions(permissionType: PermissionType.StoreEnrolement);
            viewModel.SelectedPermissions = rolePermissions.Select(e => e.Id).ToArray();
            
            if(string.IsNullOrWhiteSpace(name))
            {
                viewModel.CreateRole = true;
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("AssignPermissionsToRoleStoreEnrolment")]
        public ActionResult AssignPermissionsToRoleStoreEnrolmentPost(UpdatePermissisonsViewModel viewModel)
        {
            if (!authorizer.Authorize(Permissions.ChangeRoles))
                return new HttpUnauthorizedResult();

            var enrolmentLevel = this.enrolmentService.GetEnrolmentLevel(viewModel.Name);
            
            var allPermissions = this.permissionManager.GetInternalPermissions(permissionType: PermissionType.StoreEnrolement);
            var relatedPermissions = allPermissions.Where(e => viewModel.SelectedPermissions.Contains(e.Id));
            
            if (!this.ModelState.IsValid) 
            {
                this.notifier.Notify("Please correct the errors");
                viewModel.PossiblePermissions = allPermissions;

                return this.View(viewModel);
            }
            
            this.permissionManager.UpdatePermissions(enrolmentLevel, relatedPermissions);
            
            this.notifier.Notify("Permissions have been updated");

            return RedirectToAction("Levels", "StoreEnrolmentPermission");
        }



    }
}

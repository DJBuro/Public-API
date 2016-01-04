using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using AndroUsersDataAccess.Domain;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewSecurityGroups")]
    public class SecurityGroupController : BaseController
    {
        public SecurityGroupController()
        {
            // The Admin main menu option should be highlighted
            ViewBag.SelectedMenu = MenuItemEnum.Admin;

            // The Security Groups sub menu option should be highlighted
            ViewBag.SelectedAdminMenu = AdminMenuItemEnum.SecurityGroups;
        }

        [Security(Permissions = "ViewSecurityGroups")]
        public ActionResult Index()
        {
            try
            {
                // Get all security groups from the database
                List<SecurityGroup> securityGroups = this.SecurityGroupDAO.GetAll();

                // Create model objects that the view can use
                List<SecurityGroupModel> modelSecurityGroups = new List<SecurityGroupModel>();

                foreach (SecurityGroup securityGroup in securityGroups)
                {
                    // The andro admin administrator cannot be changed
                    if (securityGroup.Name != SecurityGroup.AdministratorSecurityGroup)
                    {
                        SecurityGroupModel securityGroupModel = new SecurityGroupModel()
                        {
                            SecurityGroup = securityGroup,
                            Permissions = null
                        };

                        if (securityGroup.Permissions != null)
                        {
                            foreach (Permission permission in securityGroup.Permissions)
                            {
                                securityGroupModel.Permissions.Add(new PermissionModel() { Permission = permission });
                            }
                        }

                        modelSecurityGroups.Add(securityGroupModel);
                    }
                }

                return View(modelSecurityGroups);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "AddSecurityGroup")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Security(Permissions = "AddSecurityGroup")]
        public ActionResult Add(SecurityGroupModel securityGroupModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Was a name provided?
                    if (securityGroupModel.SecurityGroup.Name.Length < 1 || securityGroupModel.SecurityGroup.Name.Length > 255)
                    {
                        ModelState.AddModelError("SecurityGroup.Name", "Name must be between one and 255 characters");
                        return View(securityGroupModel);
                    }

                    // Has the security group name already been used?
                    AndroUsersDataAccess.Domain.SecurityGroup existingSecurityGroup = this.SecurityGroupDAO.GetByName(securityGroupModel.SecurityGroup.Name);
                    if (existingSecurityGroup != null)
                    {
                        ModelState.AddModelError("SecurityGroup.Name", "Name already used");
                        return View(securityGroupModel);
                    }

                    // Add the security group
                    this.SecurityGroupDAO.Add(securityGroupModel.SecurityGroup);

                    // Success!
                    TempData["message"] = "Security group " + securityGroupModel.SecurityGroup.Name  + " successfully added";
                    return RedirectToAction("Index", "SecurityGroup");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Add", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "ViewSecurityGroups")]
        public ActionResult Details(int id, bool? edit)
        {
            try
            {
                // Do we need to run in edit mode?
                if (edit.HasValue && edit.Value)
                {
                    ViewBag.Edit = true;
                }
                else
                {
                    ViewBag.Edit = null;
                }

                // Get the security group details from the database
                SecurityGroupModel existingSecurityGroupModel = this.GetSecurityGroupModel(id);

                if (existingSecurityGroupModel == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                return View(existingSecurityGroupModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Details", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditSecurityGroup")]
        public ActionResult Details(Model.SecurityGroupModel securityGroupModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Was a name provided?
                    if (securityGroupModel.SecurityGroup.Name.Length < 1 || securityGroupModel.SecurityGroup.Name.Length > 255)
                    {
                        ModelState.AddModelError("SecurityGroup.Name", "Name must be between one and 255 characters");
                        return View();
                    }

                    // Has the security group name already been used?
                    AndroUsersDataAccess.Domain.SecurityGroup existingSecurityGroup = this.SecurityGroupDAO.GetByName(securityGroupModel.SecurityGroup.Name);
                    if (existingSecurityGroup != null)
                    {
                        ModelState.AddModelError("SecurityGroup.Name", "Name already used");
                        return View(securityGroupModel);
                    }

                    // Update the security group
                    this.SecurityGroupDAO.Update(securityGroupModel.SecurityGroup);

                    // Success!
                    TempData["message"] = "Security group " + securityGroupModel.SecurityGroup.Name + " successfully updated";
                    return RedirectToAction("Details", "SecurityGroup", new { id = securityGroupModel.SecurityGroup.Id });
                }
                else
                {
                    return View();
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Update", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        private SecurityGroupModel GetSecurityGroupModel(int id)
        {
            SecurityGroupModel securityGroupModel = null;

            SecurityGroup securityGroup = this.SecurityGroupDAO.GetById(id);

            if (securityGroup != null)
            {
                securityGroupModel = new SecurityGroupModel()
                {
                    SecurityGroup = securityGroup,
                    Permissions = null
                };
            }

            return securityGroupModel;
        }

        [Security(Permissions = "ViewSecurityGroups")]
        public ActionResult Permissions(int id)
        {
            try
            {
                // Get the security group
                SecurityGroup securityGroup = this.SecurityGroupDAO.GetById(id);

                if (securityGroup == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                // Build a lookup of the permissions that the security group already has to speed things up
                Dictionary<int, Permission> securityGroupPermissionsLookup = new Dictionary<int, Permission>();
                foreach (Permission permission in securityGroup.Permissions)
                {
                    securityGroupPermissionsLookup.Add(permission.Id, permission);
                }

                // Get all available permissions
                List<Permission> permissions = this.PermissionsDAO.GetAll();

                // Build a list of permission models that the view can use
                List<PermissionModel> permissionModels = new List<PermissionModel>();
                foreach (Permission permission in permissions)
                {
                    bool selected = false;
                    
                    // Does the security group already have this permission?
                    Permission securityGroupPermission = null;
                    if (securityGroupPermissionsLookup.TryGetValue(permission.Id, out securityGroupPermission))
                    {
                        // The security group already has this permission
                        selected = true;
                    }

                    permissionModels.Add(new PermissionModel() { Permission = permission, Selected = selected });
                }

                // Build a model that the view can use
                SecurityGroupModel securityGroupModel = new SecurityGroupModel()
                {
                    SecurityGroup = securityGroup,
                    Permissions = permissionModels
                };

                return View(securityGroupModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Permissions", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditSecurityGroup")]
        public ActionResult Permissions(SecurityGroupModel securityGroupModel)
        {
            try
            {
                // Get the existing security group details
                SecurityGroup existingSecurityGroup = this.SecurityGroupDAO.GetById(securityGroupModel.SecurityGroup.Id);

                if (existingSecurityGroup == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                // Build a dictionary of permissions so we can do fast lookups
                Dictionary<int, Permission> existingPermissionsLookup = new Dictionary<int, Permission>();
                if (existingSecurityGroup.Permissions != null)
                {
                    foreach (Permission permission in existingSecurityGroup.Permissions)
                    {
                        existingPermissionsLookup.Add(permission.Id, permission);
                    }
                }

                // Permissions that need to be added with the security group
                List<PermissionModel> addPermissions = new List<PermissionModel>();

                // Permissions that need to be removed from the security group
                List<PermissionModel> removePermissions = new List<PermissionModel>();

                // Go through ALL permissions and figure out whether each one should be in the security group or not
                foreach (PermissionModel permissionModel in securityGroupModel.Permissions)
                {
                    // Does the user want the permission to be associated with the security group?
                    if (permissionModel.Selected)
                    {
                        // Is the permission already associated with the security group?
                        Permission existingPermission = null;
                        if (!existingPermissionsLookup.TryGetValue(permissionModel.Permission.Id, out existingPermission))
                        {
                            // This permission needs to be associated with the security group
                            addPermissions.Add(permissionModel);
                        }
                    }
                    else
                    {
                        // Is the permission already allocated to the security group?
                        Permission existingPermission = null;
                        if (existingPermissionsLookup.TryGetValue(permissionModel.Permission.Id, out existingPermission))
                        {
                            // This permission needs to be removed from the security group
                            removePermissions.Add(permissionModel);
                        }
                    }
                }

                // Add permissions to the security group
                foreach (PermissionModel addPermissionModel in addPermissions)
                {
                    this.SecurityGroupDAO.AddPermission(securityGroupModel.SecurityGroup.Id, addPermissionModel.Permission.Id);
                }

                // Remove permissions from the security group
                foreach (PermissionModel addPermissionModel in removePermissions)
                {
                    this.SecurityGroupDAO.RemovePermission(securityGroupModel.SecurityGroup.Id, addPermissionModel.Permission.Id);
                }

                ViewBag.Edit = null;

                TempData["message"] = "Security group " + securityGroupModel.SecurityGroup.Name + " successfully updated";

                return RedirectToAction("Details", "SecurityGroup", new { id = securityGroupModel.SecurityGroup.Id, edit = false });
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("SecurityGroupController.Permissions", exception);

                return RedirectToAction("Index", "Error");
            }
        }
    }
}

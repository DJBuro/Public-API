using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.Helpers;
using AndroAdmin.Model;
using AndroUsersDataAccess.Domain;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewUsers")]
    public class AdminController : BaseController
    {
        public AdminController()
        {
            // The Admin main menu option should be highlighted
            ViewBag.SelectedMenu = MenuItemEnum.Admin;

            // The Users sub menu option should be highlighted
            ViewBag.SelectedAdminMenu = AdminMenuItemEnum.Users;
        }

        [Security(Permissions = "ViewUsers")]
        public ActionResult Index()
        {
            try
            {
                // Get all users from the database
                List<AndroUser> users = this.AndroUserDAO.GetAll();

                // Build a model that can be passed to the view
                List<UserListModel> modelUsers = new List<UserListModel>();

                foreach (AndroUser user in users)
                {
                    UserListModel userListModel = new UserListModel() { AndroUser = user };
                    foreach (SecurityGroup securityGroup in user.SecurityGroups.Values)
                    {
                        if (userListModel.SecurityGroups.Length > 0)
                        {
                            userListModel.SecurityGroups += ", ";
                        }

                        userListModel.SecurityGroups += securityGroup.Name;
                    }

                    modelUsers.Add(userListModel);
                } 
                
                return View(modelUsers);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AdminController.Index", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "AddUser")]
        public ActionResult Add(AndroUser androUser)
        {
            try
            {
                // Add a user
                this.AndroUserDAO.Add(androUser);

                // Return to the default view
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AdminController.Add", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [Security(Permissions = "EditUser")]
        public ActionResult Edit(int id)
        {
            try
            {
                // Get the user
                AndroUser androUser = this.AndroUserDAO.GetById(id);

                if (androUser == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                // A model that the view can use.  Lists ALL security groups but adds a "Selected" property so that the security groups that the user should be part of can be ticked
                List<SecurityGroupModel> securityGroups = new List<SecurityGroupModel>();

                // Get all security groups
                List<SecurityGroup> allSecurityGroups = this.SecurityGroupDAO.GetAll();
                
                // Go through the list of all security groups and select the ones that the user is already a member of
                foreach (SecurityGroup securityGroup in allSecurityGroups)
                {
                    bool selected = false;

                    foreach (SecurityGroup alreadyInSecurityGroup in androUser.SecurityGroups.Values)
                    {
                        if (alreadyInSecurityGroup.Id == securityGroup.Id)
                        {
                            selected = true;
                            break;
                        }
                    }

                    securityGroups.Add
                    (
                        new SecurityGroupModel
                        {
                            SecurityGroup = securityGroup,
                            Permissions = null,
                            Selected = selected
                        }
                    );
                }

                UserModel userModel = new UserModel()
                {
                    Id = androUser.Id,
                    EmailAddress = androUser.EmailAddress,
                    SecurityGroups = securityGroups
                };

                return View(userModel);
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AdminController.Edit", exception);

                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        [Security(Permissions = "EditUser")]
        public ActionResult Edit(UserModel userModel)
        {
            try
            {
                // Get the existing user details
                AndroUser previousAndroUser = this.AndroUserDAO.GetById(userModel.Id);

                if (previousAndroUser == null)
                {
                    return RedirectToAction("Index", "Error");
                }

                // Build a dictionary of previous security groups so we can do fast lookups
                Dictionary<int, SecurityGroup> previouslyInSecurityGroupsLookup = new Dictionary<int, SecurityGroup>();
                if (previousAndroUser.SecurityGroups != null)
                {
                    foreach (SecurityGroup securityGroup in previousAndroUser.SecurityGroups.Values)
                    {
                        previouslyInSecurityGroupsLookup.Add(securityGroup.Id, securityGroup);
                    }
                }

                // Security groups that the user should be added to
                List<SecurityGroupModel> addUserToSecurityGroups = new List<SecurityGroupModel>();

                // Security groups that the user should be removed from
                List<SecurityGroupModel> removeUserFromSecurityGroups = new List<SecurityGroupModel>();

                // Go through ALL security groups and figure out whether or not the user should be in each group
                foreach (SecurityGroupModel securityGroupModel in userModel.SecurityGroups)
                {
                    // Should the user be in the security group?
                    if (securityGroupModel.Selected)
                    {
                        // Is the user already in the security group?
                        SecurityGroup existingSecurityGroup = null;
                        if (!previouslyInSecurityGroupsLookup.TryGetValue(securityGroupModel.SecurityGroup.Id, out existingSecurityGroup))
                        {
                            // This user needs to be in the security group
                            addUserToSecurityGroups.Add(securityGroupModel);
                        }
                    }
                    else
                    {
                        // Is the user already in the security group?
                        SecurityGroup existingSecurityGroup = null;
                        if (previouslyInSecurityGroupsLookup.TryGetValue(securityGroupModel.SecurityGroup.Id, out existingSecurityGroup))
                        {
                            // The user needs to be removed from the security group
                            removeUserFromSecurityGroups.Add(securityGroupModel);
                        }
                    }
                }

                // Add the user to security groups
                foreach (SecurityGroupModel addUserToSecurityGroupModel in addUserToSecurityGroups)
                {
                    this.SecurityGroupDAO.AddUser(addUserToSecurityGroupModel.SecurityGroup.Id, userModel.Id);
                }

                // Remove the user from security groups
                foreach (SecurityGroupModel removeUserFromSecurityGroupModel in removeUserFromSecurityGroups)
                {
                    this.SecurityGroupDAO.RemoveUser(removeUserFromSecurityGroupModel.SecurityGroup.Id, userModel.Id);
                }

                ViewBag.Edit = null;

                TempData["message"] = "User " + previousAndroUser.EmailAddress + " successfully updated";

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AdminController.Edit POST", exception);

                return RedirectToAction("Index", "Error");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AndroAdmin.Model;
using AndroUsersDataAccess.DataAccess;
using AndroAdmin.DataAccess;
using AndroUsersDataAccess.Domain;
using System.Web.Security;
using AndroAdmin.Helpers;
using System.Web.UI;
using AndroAdminDataAccess.DataAccess;

namespace AndroAdmin.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(UserModel user)
        {
            ActionResult actionResult = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    actionResult = View(user);
                }

                if (actionResult == null)
                {
                    // Get an object that can talk to the database
                    IAndroUserDAO androUserDAO = AndroUsersDataAccessFactory.GetAndroUserDAO();

                    // Get the user
                    AndroUser androUser = androUserDAO.GetByEmailAddress(user.EmailAddress);

                    // Does the user exist?
                    if (androUser == null)
                    {
                        // Unknown user
                        ModelState.AddModelError("EmailAddress", "Unknown email address or invalid password");

                        actionResult = View();
                    }
                    else
                    {
                        // Was the correct password provided
                        if (androUser.Password != user.Password)
                        {
                            // Invalid password
                            ModelState.AddModelError("EmailAddress", "Unknown email address or invalid password");

                            actionResult = View(user);
                        }
                        else
                        {
                            // Credentials correct
                            FormsAuthentication.SetAuthCookie(user.EmailAddress, true);

                            actionResult = RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.LogError("AccountController.LogOn (POST)", exception);

                actionResult = RedirectToAction("Index", "Error");
            }

            return actionResult;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LogOn", "Account");
        }
    }
}

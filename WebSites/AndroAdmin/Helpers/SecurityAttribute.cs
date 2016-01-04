using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AndroAdmin.DataAccess;
using AndroUsersDataAccess.DataAccess;

namespace AndroAdmin.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SecurityAttribute : AuthorizeAttribute
    {
        // This property can be set as part of the "Security" attribute on each controller class or controller method e.g. 
        // [Authorize]
        // [Security(Permissions="ViewACSPartners")]
        // public class PartnerController : BaseController
        public string Permissions { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            // Check to see if the user is authorized to view this page
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                // User is not authorized to view this page.  Is the user logged in?
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    // User is logged in but does not have permission to access the page
                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                    redirectTargetDictionary.Add("action", "NotAuthorized");
                    redirectTargetDictionary.Add("controller", "Error");

                    // Redirect the the not authorized page
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                }
                else
                {
                    // User not logged in.  Redirect to Login page.
                    HandleUnauthorizedRequest(filterContext);
                }
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Does the user have permission to access this page?
            return this.IsAuthorized(httpContext);
        }

        private bool IsAuthorized(HttpContextBase httpContext)
        {
            bool authorized = false;

            if (!string.IsNullOrEmpty(this.Permissions))
            {
                string[] requiredRoles = this.Permissions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> requiredRolesList = requiredRoles.ToList<string>();

                IPermissionDAO permissionsDAO = AndroUsersDataAccessFactory.GetPermissionsDAO();
                //           IPermissionDAO permissionsDAO.ConnectionStringOverride = this.AndroUsersConnectionStringOverride;

                // Get a list of all the users permissions
                List<string> permissions = permissionsDAO.GetNamesByUserName(httpContext.User.Identity.Name);

                foreach (string requiredRole in requiredRoles)
                {
                    foreach (string permission in permissions)
                    {
                        if (requiredRole.Trim().Equals(permission.Trim(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            requiredRolesList.Remove(requiredRole);
                            break;
                        }
                    }
                }

                if (requiredRolesList.Count == 0)
                {
                    authorized = true;
                }
            }

            return authorized;
        }
    }
}
using System;
using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using WebDashboard.Dao;
using WebDashboard.Mvc;
using WebDashboard.Mvc.Filters;
using WebDashboard.Web.Models;
using System.Web;

namespace WebDashboard.Web.Controllers
{

    [HandleError]
    public class AccountController : SiteController
    {

        public IUserDao UserDao { get; set; }

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult Executive()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult Executive(string userName, string password)
        {

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            var data = new WebDashboardViewData.PageViewData();

            var user = UserDao.FindByEmailAndPassword(userName, password);

            if (user == null)
            {
                ModelState.AddModelError("username", WebDashboard.Web.ResourceHelper.GetString("InvalidLogin"));
                return View();
            }

            if (user.IsExecutiveDashboardUser)
            {
                Mvc.Utilities.Cookie.SetAuthoriationCookie(user, HttpContext.Response);
                data.User = user;
            }
            else
            {
                ModelState.AddModelError("username", WebDashboard.Web.ResourceHelper.GetString("NotHeadOfficeUser"));
                return View();
            }

            return RedirectToAction("Index", "Executive");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password)
        {

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            var data = new WebDashboardViewData.PageViewData();

            var user = UserDao.FindByEmailAndPassword(userName, password);
            if(user != null)
            {
                Mvc.Utilities.Cookie.SetAuthoriationCookie(user,HttpContext.Response);
                data.User = user;
            }
            else
            {
                ModelState.AddModelError("username", WebDashboard.Web.ResourceHelper.GetString("InvalidLogin"));
                return View();
            }

            if (user.IsExecutiveDashboardUser)
            {
                return RedirectToAction("Index", "Executive");
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

       
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            HttpContext.Session["Culture"] = new CultureInfo(lang);

            return Redirect(returnUrl);
        }

        #region Validation Methods

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }

            return ModelState.IsValid;
        }

        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }
}

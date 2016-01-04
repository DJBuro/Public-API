using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using WebMatrix.WebData;
using MVCAndromeda.Models;

namespace MVCAndromeda.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly IUserDataService userDataService;
        private readonly IUserChainsDataService userChainsDataService;
        
        public AccountController(IUserDataService userDataService, IUserChainsDataService userChainsDataService)
        {
            this.userChainsDataService = userChainsDataService;
            this.userDataService = userDataService;
        }

        //
        // GET: /Account/Login

        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Message = "Welcome to Androreports!";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl, string today)
        {
            ViewBag.Message = "Welcome to Androreports!";
          
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!Membership.ValidateUser(model.UserName, model.Password)) 
            {
                //this.ModelState.AddModelError("UserName", new Exception("Invalid Credentials"));
                ViewBag.invalidCredentials = true;
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    
            MyAndromedaUser myAndromedaUser = this.userDataService.GetByUserName(model.UserName);
           // Session["Owner Name"] = string.Format("{0} {1}", myAndromedaUser.Firstname, myAndromedaUser.Surname);
            Session["Owner Name"] = myAndromedaUser.Firstname;
            //WARNING: USING YESTERDAY. REMOVE ADDDAYS
            Session["Today"] = (Convert.ToDateTime(today)).AddDays(-1);
            var chainNames = this.userChainsDataService.GetChainsForUser(myAndromedaUser.Id).Select(e=> e.Name).ToList();
            Session["Chains"] = chainNames;
            if (Session["cube"] == null) Session["cube"] = new CubeAdapter();

            return RedirectToAction("Index", "StoreData");
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            // ATTENTION
            // this is modified

            if (Session["cube"] != null)
            {
                Session.Remove("cube");
            }
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff(int i)
        {
            FormsAuthentication.SignOut();
            // ATTENTION
            // this is modified

            if (Session["cube"] != null)
            {
                Session.Remove("cube");
            }
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        //internal class ExternalLoginResult : ActionResult
        //{
        //    public ExternalLoginResult(string provider, string returnUrl)
        //    {
        //        Provider = provider;
        //        ReturnUrl = returnUrl;
        //    }

        //    public string Provider { get; private set; }
        //    public string ReturnUrl { get; private set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        //    }
        //}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Models.Account;
using MyAndromeda.Web.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;
using MyAndromeda.Framework.Mvc;
using MyAndromeda.Authorization.Management;
using System.Web;
using System.Monads;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;

namespace MyAndromeda.Web.Controllers
{
    public class AccountController : Controller, IRedirectController
    {
        private readonly IWorkContext workContext;
        private readonly INotifier notifier;
        
        private readonly IUserDataService userDataService;
        private readonly ILoginService loginServicee;
        private readonly IPasswordResetService passwordResetService;
        private readonly IPasswordStrengthService passwordStrengthService;
        private readonly MyAndromedaMembershipProvider.MyAndromedaMembershipProvider membershipProvider;
        
        public AccountController(IWorkContext workContext, IMyAndromedaLogger logger, INotifier notifier, ILoginService loginServicee, MyAndromedaMembershipProvider.MyAndromedaMembershipProvider membershipProvider, ITranslator translator, IUserDataService userDataService, IPasswordResetService passwordResetService, IPasswordStrengthService passwordStrengthService)
        {
            this.passwordStrengthService = passwordStrengthService;
            this.workContext = workContext;
            this.passwordResetService = passwordResetService;
            this.userDataService = userDataService;
            this.membershipProvider = membershipProvider;
            this.loginServicee = loginServicee;
            this.notifier = notifier;
            this.Logger = logger;
            this.Translator = translator;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IMyAndromedaLogger Logger { get; private set; }

        public ITranslator Translator { get; private set; }

        public RedirectResult Redirect(string returnUrl)
        {
            return base.Redirect(returnUrl);
        }

        public RedirectToRouteResult RedirectToAction(string p, string p1, object p2)
        {
            return base.RedirectToAction(p, p1, p2);
        }

        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgottenPassword() 
        {
            var model = new Models.Account.ForgottonPasswordModel();
            return View(model);
        }

        [AllowAnonymous, HttpPost, ActionName("ForgottenPassword"), ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgottenPasswordPost(Models.Account.ForgottonPasswordModel model) 
        {
            if (string.IsNullOrWhiteSpace(model.Email)) 
            {
                this.ModelState.AddModelError("Email", Translator.T("Please enter a valid username"));
                return View(model);
            }

            var user = this.userDataService.GetByUserName(model.Email);

            if (user == null) 
            {
                this.ModelState.AddModelError("Email" , Translator.T("Please enter a valid username"));
                return View(model);
            }

            var code = this.passwordResetService.GetResetCode(user.Username);
            var email = new Models.Emails.ResetPasswordEmail() { FirstName = user.Firstname, Email = user.Username, ValidationCode = code };

            await email.SendAsync();

            this.notifier.Notify(Translator.T("A password reset link has been sent to your email address. This link will be valid for 12 more hours"));
            
            return RedirectToAction("LogOn");
        }

        [AllowAnonymous, HttpGet]
        public ActionResult ResetPassword(string code) 
        {
            code = HttpUtility.UrlDecode(code);

            if (User.Identity.IsAuthenticated && workContext.CurrentUser.Available) 
            {
                return RedirectToAction("Index", "Home");
            }

            if (!this.passwordResetService.VerifyCode(code)) 
            {
                this.notifier.Error(Translator.T("Your email reset link has expired. Please generate another."));
                return RedirectToAction("ForgottenPassword");
            }

            var userName = this.passwordResetService.GetUserNameFromText(code);
            var model = new ResetPasswordModel()
            {
                Email = userName,
                Password = string.Empty
            };

            return View(model);
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<ActionResult> ResetPasswordPost(ResetPasswordModel model) 
        {
            model.CheckNull("model");
            model.Email.CheckNull("Email");
            model.Code.CheckNull("Code");

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return View(model);   
            }
            if (!this.passwordResetService.VerifyCode(model.Code)) 
            {
                this.notifier.Error(Translator.T("Your email reset link has expired. Generate another from the login page"));
                return RedirectToAction("LogOn");
            }

            var passwordErrors = this.passwordStrengthService.ProblemsWithPassword(model.Password).ToArray();

            if (passwordErrors.Length > 0) 
            {
                foreach (var error in passwordErrors) 
                {
                    this.ModelState.AddModelError("Password", Translator.T(error));
                }

                return View(model);
            }

            var user = this.userDataService.GetByUserName(model.Email);
            this.membershipProvider.ChangePassword(model.Email, string.Empty, model.Password);

            this.notifier.Notify(Translator.T("Your password has been changed and you can now login with the new password"));
            var notificationEmail = new Models.Emails.ResetPasswordNotificationEmail() 
            {
                Email = model.Email,
                FirstName = user.Firstname
            };

            await notificationEmail.SendAsync();

            return RedirectToAction("LogOn"); 
        }

        //
        // POST: /Account/LogOn
        [HttpPost, AllowAnonymous]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            ActionResult result; 

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            Logger.Debug("Validating User {0}", model.UserName);
            if (membershipProvider.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                result = this.loginServicee.LoggedIn(this as IRedirectController, model.UserName, returnUrl);
                
                if (result == null) 
                {
                    return View(model);
                }

                return result;
            }
    
            ModelState.AddModelError("", Translator.T("The credentials supplied were incorrect."));

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        
        [Authorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel() { 
                NewPassword = string.Empty,
                ConfirmPassword= string.Empty
            };

            return View(model);
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePasswordPost(ChangePasswordModel model)
        {
            var passwordErrors = this.passwordStrengthService.ProblemsWithPassword(model.NewPassword).ToArray();

            if (passwordErrors.Length > 0)
            {
                foreach (var error in passwordErrors)
                {
                    this.ModelState.AddModelError("Password", Translator.T(error));
                }

                return View(model);
            }

            if (!ModelState.IsValid) { return View(model); }

            // ChangePassword will throw an exception rather
            // than return false in certain failure scenarios.
            bool changePasswordSucceeded;
            try
            {
                //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                //var user = this.membershipProvider.GetUserNameByEmail(User.Identity.Name);
                changePasswordSucceeded = this.membershipProvider.ChangePassword(User.Identity.Name, string.Empty, model.NewPassword);
            }
            catch (Exception)
            {
                this.notifier.Error("There was a problem setting your password. Please try again");
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                this.notifier.Notify("Your password has been successfully changed.");
                model.ConfirmPassword = string.Empty;
                model.NewPassword = string.Empty;

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //#region Status Codes
        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}
        //#endregion
    }
}

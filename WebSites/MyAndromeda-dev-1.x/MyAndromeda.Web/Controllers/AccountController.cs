using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Models.Account;
using MyAndromeda.Web.Services;
using MyAndromeda.Framework.Mvc;
using MyAndromeda.Authorization.Management;
using System.Web;
using System.Monads;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess.Users;
using MyAndromedaMembershipProvider;
using MyAndromeda.Web.Helper;
using MyAndromeda.Web.Areas.Users.Services;
using MyAndromeda.Framework.Services;

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
        private readonly ISignInHelper signUpHelper;
        private readonly IUserManagementService userManagementService;
        //private readonly MyAndromedaMembershipProvider.MyAndromedaMembershipProvider membershipProvider;

        public AccountController(IWorkContext workContext,
            IMyAndromedaLogger logger,
            INotifier notifier,
            ILoginService loginServicee,
            ISignInHelper signUpHelper,
            IUserManagementService userManagementService,
            // MyAndromedaMembershipProvider.MyAndromedaMembershipProvider membershipProvider, 
            ITranslator translator,
            IUserDataService userDataService,
            IPasswordResetService passwordResetService,
            IPasswordStrengthService passwordStrengthService)
        {
            this.passwordStrengthService = passwordStrengthService;
            this.workContext = workContext;
            this.passwordResetService = passwordResetService;
            this.userDataService = userDataService;
            this.signUpHelper = signUpHelper;
            this.userManagementService = userManagementService;
            // this.membershipProvider = membershipProvider;
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

        public new RedirectResult Redirect(string returnUrl)
        {
            return base.Redirect(returnUrl);
        }

        public new RedirectToRouteResult RedirectToAction(string p, string p1, object p2)
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
                this.ModelState.AddModelError(key: "Email", errorMessage: Translator.T(text: "Please enter a valid username"));
                return View(model);
            }

            var user = this.userDataService.GetByUserName(model.Email);

            if (user == null)
            {
                this.ModelState.AddModelError(key: "Email", errorMessage: Translator.T(text: "Please enter a valid username"));
                return View(model);
            }

            var code = this.passwordResetService.GetResetCode(user.Username);
            var email = new Models.Emails.ResetPasswordEmail() { FirstName = user.Firstname, Email = user.Username, ValidationCode = code };

            await email.SendAsync();

            this.notifier.Notify(Translator.T(text: "A password reset link has been sent to your email address. This link will be valid for 12 more hours"));

            return RedirectToAction(actionName: "LogOn");
        }

        [AllowAnonymous, HttpGet]
        public ActionResult ResetPassword(string code)
        {
            code = HttpUtility.UrlDecode(code);

            if (User.Identity.IsAuthenticated && workContext.CurrentUser.Available)
            {
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            if (!this.passwordResetService.VerifyCode(code))
            {
                this.notifier.Error(Translator.T(text: "Your email reset link has expired. Please generate another."));
                return RedirectToAction(actionName: "ForgottenPassword");
            }

            string userName = this.passwordResetService.GetUserNameFromText(code);
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
            model.CheckNull(argumentName: "model");
            model.Email.CheckNull(argumentName: "Email");
            model.Code.CheckNull(argumentName: "Code");

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return View(model);
            }
            if (!this.passwordResetService.VerifyCode(model.Code))
            {
                this.notifier.Error(Translator.T(text: "Your email reset link has expired. Generate another from the login page"));
                return RedirectToAction(actionName: "LogOn");
            }

            var passwordErrors = this.passwordStrengthService.ProblemsWithPassword(model.Password).ToArray();

            if (passwordErrors.Length > 0)
            {
                foreach (var error in passwordErrors)
                {
                    this.ModelState.AddModelError(key: "Password", errorMessage: Translator.T(error));
                }

                return View(model);
            }

            var user = this.userDataService.GetByUserName(model.Email);
            this.userManagementService.ResetPassword(user.Id, model.Password);

            this.notifier.Notify(Translator.T(text: "Your password has been changed and you can now login with the new password"));
            var notificationEmail = new Models.Emails.ResetPasswordNotificationEmail()
            {
                Email = model.Email,
                FirstName = user.Firstname
            };

            await notificationEmail.SendAsync();

            return RedirectToAction(actionName: "LogOn");
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

            Logger.Debug(format: "Validating User {0}", args: new object[] { model.UserName });
            if (signUpHelper.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                result = this.loginServicee.LoggedIn(this as IRedirectController, model.UserName, returnUrl);

                if (result == null)
                {
                    return View(model);
                }

                return result;
            }

            ModelState.AddModelError(key: string.Empty, errorMessage: Translator.T(text: "The credentials supplied were incorrect."));

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel()
            {
                NewPassword = string.Empty,
                ConfirmPassword = string.Empty
            };

            return View(model);
        }

        //
        // POST: /Account/ChangePassword
        //[Authorize]
        //[HttpPost]
        //[ActionName("ChangePassword")]
        //public ActionResult ChangePasswordPost(ChangePasswordModel model)
        //{
        //    string[] passwordErrors = this.passwordStrengthService.ProblemsWithPassword(model.NewPassword).ToArray();

        //    if (passwordErrors.Length > 0)
        //    {
        //        foreach (var error in passwordErrors)
        //        {
        //            this.ModelState.AddModelError(key: "Password", errorMessage: Translator.T(error));
        //        }

        //        return View(model);
        //    }

        //    if (!ModelState.IsValid) { return View(model); }

        //    // ChangePassword will throw an exception rather
        //    // than return false in certain failure scenarios.
        //    bool changePasswordSucceeded;
        //    try
        //    {
        //        //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
        //        //var user = this.membershipProvider.GetUserNameByEmail(User.Identity.Name);
        //        changePasswordSucceeded = this.membershipProvider.ChangePassword(User.Identity.Name, string.Empty, model.NewPassword);
        //    }
        //    catch (Exception)
        //    {
        //        this.notifier.Error(message: "There was a problem setting your password. Please try again");
        //        changePasswordSucceeded = false;
        //    }

        //    if (changePasswordSucceeded)
        //    {
        //        this.notifier.Notify(message: "Your password has been successfully changed.");
        //        model.ConfirmPassword = string.Empty;
        //        model.NewPassword = string.Empty;

        //        return View(model);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(key: string.Empty, errorMessage: "The current password is incorrect or the new password is invalid.");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

    }
}

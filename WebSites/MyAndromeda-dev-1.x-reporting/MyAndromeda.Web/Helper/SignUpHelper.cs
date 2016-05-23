using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyAndromeda.Framework.Services;
using MyAndromeda.Identity;
using System;
using System.Web;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Web.Helper
{
    public class SignInHelper : ISignInHelper
    {
        private readonly IMembershipService membershipService;

        public SignInHelper(
            ApplicationUserManager userManager,
            //IAuthenticationManager authManager, 
            IMembershipService membershipService)
        {
            this.membershipService = membershipService;
            //this.AuthenticationManager = authManager;
            this.UserManager = userManager;
        }

        public ApplicationUserManager UserManager { get; private set; }
        // public IAuthenticationManager AuthenticationManager { get; private set; }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public async Task SignInAsync(MyAndromedaIdentityUser user, bool isPersistent, bool rememberBrowser)
        {
            // Clear any partial cookies from external or two factor partial sign ins
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);

            var userIdentity = await user.GenerateUserIdentityAsync(UserManager);

            if (rememberBrowser)
            {
                //var rememberBrowserIdentity =
                //    AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(user.Id);

                AuthenticationManager.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    userIdentity);
            }
            else
            {
                AuthenticationManager.SignIn(
                    new AuthenticationProperties { IsPersistent = isPersistent },
                    userIdentity);
            }
        }

        public async Task<bool> SendTwoFactorCode(string provider)
        {
            var userId = await GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return false;
            }

            var token = await UserManager.GenerateTwoFactorTokenAsync(userId.GetValueOrDefault(), provider);

            // See IdentityConfig.cs to plug in Email/SMS services to actually send the code
            // await UserManager.NotifyTwoFactorTokenAsync(userId, provider, token);
            return true;
        }

        public async Task<int?> GetVerifiedUserIdAsync()
        {
            var result = await AuthenticationManager.AuthenticateAsync(
                DefaultAuthenticationTypes.TwoFactorCookie);
            int id = 0;
            if (result != null && result.Identity != null
                && !String.IsNullOrEmpty(result.Identity.GetUserId()))
            {
                var isSuccessful = int.TryParse(result.Identity.GetUserId(), out id);
                if (isSuccessful)
                {
                    return id;
                }
            }
            return null;
        }

        public async Task<bool> HasBeenVerified()
        {
            return await GetVerifiedUserIdAsync() != null;
        }

        public async Task<SignInStatus> TwoFactorSignIn(
            string provider,
            string code,
            bool isPersistent,
            bool rememberBrowser)
        {
            var userId = await GetVerifiedUserIdAsync();

            if (userId == null)
            {
                return SignInStatus.Failure;
            }

            var user = await UserManager.FindByIdAsync(userId.GetValueOrDefault());

            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            if (await UserManager.VerifyTwoFactorTokenAsync(user.Id, provider, code))
            {
                // When token is verified correctly, clear the access failed 
                // count used for lockout
                await UserManager.ResetAccessFailedCountAsync(user.Id);
                await SignInAsync(user, isPersistent, rememberBrowser);
                return SignInStatus.Success;
            }

            // If the token is incorrect, record the failure which 
            // also may cause the user to be locked out
            await UserManager.AccessFailedAsync(user.Id);
            return SignInStatus.Failure;
        }

        public async Task<SignInStatus> ExternalSignIn(
            ExternalLoginInfo loginInfo,
            bool isPersistent)
        {
            var user = await UserManager.FindAsync(loginInfo.Login);

            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            return await SignInAsync(user, isPersistent);
        }

        private async Task<SignInStatus> SignInAsync(MyAndromedaIdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));

            return SignInStatus.Success;
        }

        public async Task<SignInStatus> PasswordSignIn(
            string userName,
            string password,
            bool isPersistent,
            bool shouldLockout)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            if (await UserManager.CheckPasswordAsync(user, password))
            {
                return await SignInAsync(user, isPersistent);
            }

            if (shouldLockout)
            {
                // If lockout is requested, increment access failed 
                // count which might lock out the user
                await UserManager.AccessFailedAsync(user.Id);

                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return SignInStatus.LockedOut;
                }
            }
            return SignInStatus.Failure;
        }

        public bool ValidateUser(string username, string password)
        {
            var result = this.membershipService.ValidateUserLogin(username, password);

            bool success = result != null;

            return success;
        }

        public bool ChangePassword(string username, string password)
        {
            var user = this.membershipService.GetUserRecord(username);

            var isPasswordSetted = this.membershipService.SetPassword(user, password, true);
            return isPasswordSetted;
        }
    }
}
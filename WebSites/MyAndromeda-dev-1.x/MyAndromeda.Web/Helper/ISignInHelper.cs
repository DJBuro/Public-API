using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using MyAndromeda.Core;
using MyAndromeda.Identity;

namespace MyAndromeda.Web.Helper
{
    public interface ISignInHelper : IDependency
    {
        ApplicationUserManager UserManager { get; }

        Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistent);

        Task<int?> GetVerifiedUserIdAsync();

        Task<bool> HasBeenVerified();

        Task<SignInStatus> PasswordSignIn(string userName, string password, bool isPersistent, bool shouldLockout);

        Task<bool> SendTwoFactorCode(string provider);

        Task SignInAsync(MyAndromedaIdentityUser user, bool isPersistent, bool rememberBrowser);

        Task<SignInStatus> TwoFactorSignIn(string provider, string code, bool isPersistent, bool rememberBrowser);

        bool ValidateUser(string username, string password);
    }
}
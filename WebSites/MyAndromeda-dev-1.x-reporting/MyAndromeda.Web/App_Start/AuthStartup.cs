using System;
using Owin;
using Microsoft.Owin;
using MyAndromeda.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;
using MyAndromeda.Web.Providers;

[assembly: OwinStartup(typeof(MyAndromeda.Web.AppStart.SignalrStartup))]
namespace MyAndromeda.Web.AppStart
{
    public class AuthStartup
    {
        public const string PublicClientId = "MyAndromeda-Web-V1";

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<MyAndromedaIdentityRoleStore>(MyAndromedaIdentityRoleStore.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider

            var cookieAuthentication = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(value: "/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user 
                    // logs in. This is a security feature which is used when you 
                    // change a password or add an external login to your account.  

                    OnValidateIdentity = SecurityStampValidator
                        // ADD AN INT AS A THIRD TYPE ARGUMENT:

                        .OnValidateIdentity<ApplicationUserManager, MyAndromedaIdentityUser, int>(
                            validateInterval: TimeSpan.FromMinutes(value: 30),
                            regenerateIdentityCallback: (manager, user)
                                => user.GenerateUserIdentityAsync(manager), getUserIdCallback: (claim) => int.Parse(claim.GetUserId())
                        )
                }

            };

            app.UseCookieAuthentication(cookieAuthentication);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(value: "/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString(value: "/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(value: 7),
                AllowInsecureHttp = true,
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
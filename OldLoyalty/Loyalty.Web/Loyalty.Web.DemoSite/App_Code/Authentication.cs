using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
//using LoyaltyAdmin;
//using LoyaltyAdmin;
using serviceLoyaltyAccount=LoyaltyWS.serviceLoyaltyAccount;

/// <summary>
/// Summary description for Authentication
/// </summary>
public static class Authentication
{
    private const string CONTEXT_USER_LOGGED_IN_FLAG = "user_logged_in";

    public const string LoyaltyCardUserCookie = "LoyaltyCardUserId"; 
    public const string LoyaltyCardCompanyCookie = "LoyaltyCompanyId"; 

    public static class AuthCookie
    {
        public const int ExpirationMinutes = 20;
    }

    public static void CreateUser(string userName, string password, string emailAddress)
    {
        MembershipCreateStatus createStatus;
        MembershipUser newUser = Provider.CreateUser(userName, password, emailAddress, null, null, true, null, out createStatus);
    }

    public static void SetUserCookie(this System.Web.HttpResponse response, serviceLoyaltyAccount account)
    {
        
        //todo: log User Activity?
        //UserDAO.LogUserLastActivity(account);

        SetAuthoriationCookie(response,account); //session auth cookie

        response.Cookies.Set(new HttpCookie(LoyaltyCardUserCookie, account.Id.Value.ToString())); //set a userId cookie
        
        HttpContext.Current.Items[CONTEXT_USER_LOGGED_IN_FLAG] = true;
    }

    public static void SetAuthoriationCookie(this HttpResponse response,serviceLoyaltyAccount account)
    {

        var authTicket = new FormsAuthenticationTicket(1, account.Id.Value.ToString(), DateTime.Now,
                                                                             DateTime.Now.AddMinutes(AuthCookie.ExpirationMinutes), false,
                                                                             account.LoyaltyUser.Id.Value.ToString());
        // encrypt the ticket.
        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

        // Create a cookie and add the encrypted ticket to the cookie as data.
        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

        response.SetCookie(new HttpCookie(LoyaltyCardUserCookie, account.LoyaltyUser.Id.Value.ToString())); //set a userId cookie

        response.SetCookie(authCookie);

    }

    //public static void SetAuthoriationCookie(this HttpResponse response, serviceCompany company)
    //{

    //    var authTicket = new FormsAuthenticationTicket(1, company.Id.Value.ToString(), DateTime.Now,
    //                                                                         DateTime.Now.AddMinutes(AuthCookie.ExpirationMinutes), false,
    //                                                                         company.Id.Value.ToString());
    //    // encrypt the ticket.
    //    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

    //    // Create a cookie and add the encrypted ticket to the cookie as data.
    //    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

    //    response.SetCookie(new HttpCookie(LoyaltyCardCompanyCookie, company.Id.Value.ToString())); //set a userId cookie

    //    response.SetCookie(authCookie);

    //}

    public static bool GetAuthorisationCookie(this HttpRequest request)
    {
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = request.Cookies[cookieName];

        if (null == authCookie)
        {
            // There is no authentication cookie.
            return false;
        }

        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        var id = new FormsIdentity(authTicket);

        string[] roles = authTicket.UserData.Split(new char[] { '|' });

        var principal = new GenericPrincipal(id, roles);

        HttpContext.Current.User = principal;

        return true;
    }

    public static int GetAuthoriationCookieLoyaltyAccountId(this HttpRequest request)
    {
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = request.Cookies[cookieName];

        if (null == authCookie)
        {
            // There is no authentication cookie.
            return -1;
        }

        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        var id = new FormsIdentity(authTicket);

        string[] roles = authTicket.UserData.Split(new char[] { '|' });

        var principal = new GenericPrincipal(id, roles);

        HttpContext.Current.User = principal;

        return Convert.ToInt32(principal.Identity.Name);
    }

    public static int GetAuthoriationCookieLoyaltyCompanyId(this HttpRequest request)
    {
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = request.Cookies[cookieName];

        if (null == authCookie)
        {
            // There is no authentication cookie.
            return -1;
        }

        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

        var id = new FormsIdentity(authTicket);

        string[] roles = authTicket.UserData.Split(new char[] { '|' });

        var principal = new GenericPrincipal(id, roles);

        HttpContext.Current.User = principal;

        return Convert.ToInt32(principal.Identity.Name);
    }


 
    public static IFormsAuthentication FormsAuth
    {
        get;
        private set;
    }

    public static MembershipProvider Provider
    {
        get;
        private set;
    }

    public interface IFormsAuthentication
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationWrapper : IFormsAuthentication
    {

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    //todo: implement
    private static string ErrorCodeToString(MembershipCreateStatus createStatus)
    {
        // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
        // a full list of status codes.
        switch (createStatus)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return "Username already exists. Please enter a different user name.";

            case MembershipCreateStatus.DuplicateEmail:
                return "A username for that e-mail address already exists. Please enter a different e-mail address.";

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
}

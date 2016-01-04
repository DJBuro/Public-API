using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Mvc.Utilities
{
    public static class AuthCookie
    {
        //note: 12 hours enough?
        public const int ExpirationMinutes = 720;
    }


    public class Cookie
    {

        public static void SetCookieParam(string paramname, string paramvalue, HttpResponse response)
        {

            var cookie = new HttpCookie(paramname, paramvalue);
            response.SetCookie(cookie);
        }

        public static void SetCookieParam(string paramname, string paramvalue, HttpResponse response, DateTime expiry)
        {
            var cookie = new HttpCookie(paramname, paramvalue);
            cookie.Expires = expiry;
            response.SetCookie(cookie);
        }

        public static string GetCookieParam(string paramName, HttpRequestBase request)
        {
            var cookie = request.Cookies[paramName];
            if (cookie == null) return null;
            return cookie.Value;
        }

        public static string GetLanguageCookie(HttpRequestBase request)
        {
            var langCookie = request.Cookies["lang"];

            if (langCookie == null)
            {
                // There is no cookie, default to english.
                return "en";
            }

            return langCookie.Value;
        }

        #region Authorisation Cookie

        public static void SetAuthoriationCookie(User user, HttpResponseBase response)
        {
            var userData = user.Id.Value.ToString();

            var authTicket = new FormsAuthenticationTicket(1, user.EmailAddress, DateTime.Now,
                                                                                 DateTime.Now.AddMinutes(AuthCookie.ExpirationMinutes), false,
                                                                                 userData);
            // encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            response.SetCookie(authCookie);

        }

        public static void SetAuthoriationCookie(Site site, HttpResponseBase response)
        {
            var userId = site.Id.Value.ToString();

            var authTicket = new FormsAuthenticationTicket(1, site.Name, DateTime.Now,
                                                                                 DateTime.Now.AddMinutes(AuthCookie.ExpirationMinutes), false,
                                                                                 userId);
            // encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            response.SetCookie(authCookie);

        }

        public static bool GetAuthoriationCookie(HttpRequestBase request)
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

        public static int GetAuthoriationCookieSiteId(HttpRequestBase request)
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

            return Convert.ToInt32(roles[0]);
        }

        #endregion

    }
}
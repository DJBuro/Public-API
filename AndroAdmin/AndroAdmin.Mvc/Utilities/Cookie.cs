using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AndroAdmin.Dao.Domain;

namespace AndroAdmin.Mvc.Utilities
{
    public static class AuthCookie
    {
        //note: 2 hours enough?
        public const int ExpirationMinutes = 120;
    }

    //todo: read SSO
    //http://blogs.neudesic.com/blogs/michael_morozov/archive/2006/03/17/72.aspx
    //http://www.codeproject.com/KB/aspnet/SingleSignon.aspx  

    public class Cookie
    {
        public static void SetCookieParam(string paramname, string paramvalue, HttpResponseBase response)
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

        #region Authorisation Cookie

        public static void SetAuthoriationCookie(AndroUser androUser, HttpResponseBase response)
        {
            var userId = androUser.Id.Value.ToString();

            var authTicket = new FormsAuthenticationTicket(1, androUser.FirstName, DateTime.Now,
                                                                                 DateTime.Now.AddMinutes(AuthCookie.ExpirationMinutes), false,
                                                                                 userId);
            // encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            response.SetCookie(authCookie);

        }

        //todo: cross site authentication??
        public static bool GetAuthoriationCookie(HttpRequestBase request)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = request.Cookies[cookieName];

            if (null == authCookie)
            {
                // There is no authentication cookie.
                return false;
            }
            
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            
            var id = new FormsIdentity(authTicket);

            var roles = authTicket.UserData.Split(new char[] { '|' });

            var principal = new GenericPrincipal(id, roles);

            HttpContext.Current.User = principal;

            return true;
        }

        public static int GetAuthoriationCookieUserId(HttpRequestBase request)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = request.Cookies[cookieName];

            if (null == authCookie)
            {
                // There is no authentication cookie.
                return -1;
            }

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var id = new FormsIdentity(authTicket);

            var roles = authTicket.UserData.Split(new char[] { '|' });

            var principal = new GenericPrincipal(id, roles);

            HttpContext.Current.User = principal;

            return Convert.ToInt32(roles[0]);
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

        #endregion

    }
}

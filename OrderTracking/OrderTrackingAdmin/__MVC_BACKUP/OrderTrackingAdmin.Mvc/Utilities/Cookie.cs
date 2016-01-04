using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace OrderTrackingAdmin.Mvc.Utilities
{

    //todo: read SSO
    //http://blogs.neudesic.com/blogs/michael_morozov/archive/2006/03/17/72.aspx
    //http://www.codeproject.com/KB/aspnet/SingleSignon.aspx  

    public class Cookie
    {

        #region Authorisation Cookie

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

        private static void SetCookieParam(string paramname, string paramvalue, HttpResponseBase response)
        {
            var cookie = new HttpCookie(paramname, paramvalue);
            response.SetCookie(cookie);
        }

        public static void SetLanguageCookie(string language,HttpResponseBase response)
        {
            SetCookieParam("lang", language, response);
        }

        #endregion

    }
}
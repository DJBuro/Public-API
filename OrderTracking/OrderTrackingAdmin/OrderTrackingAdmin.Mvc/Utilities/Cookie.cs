using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace OrderTrackingAdmin.Mvc.Utilities
{
    public class Cookie
    {
        #region Authorisation Cookie

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
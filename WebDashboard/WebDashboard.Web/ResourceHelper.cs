using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace WebDashboard.Web
{
    public static class ResourceHelper
    {
        public static string GetString(string key)
        {
            CultureInfo currentCulture = ResourceHelper.GetCurrentCulture();

            string value = Resx.Resources.ResourceManager.GetString(key, currentCulture);
            
            return value;
        }
        
        private static CultureInfo GetCurrentCulture()
        {
            CultureInfo currentCulture = null;
            
            if (HttpContext.Current.Session != null)
            {
                currentCulture = (CultureInfo)HttpContext.Current.Session["Culture"];

                if (currentCulture == null)
                {
                    currentCulture = ResourceHelper.GetDefaultCulture();

                    HttpContext.Current.Session["Culture"] = currentCulture;
                }
            }
            else
            {
                currentCulture = ResourceHelper.GetDefaultCulture();
            }

            return currentCulture;
        }
        
        private static CultureInfo GetDefaultCulture()
        {
            string langName = "en";

            if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
            {
                langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
            }

            return new CultureInfo(langName);
        }
    }
}

using System;
using System.Configuration;
using System.Linq;

namespace MyAndromeda.Web.Helper
{
    public static class SettingsHelper
    {
        private static readonly string debugModeKey = "StagingApplicationMode";
        private static readonly string debugEmailsToValue = "StagingMailModeToValue";

        public static bool DebugMode 
        {
            get 
            {
                var value = ConfigurationManager.AppSettings[debugModeKey];

                if (value == null)
                    return false;

                return Convert.ToBoolean(value);
            }
        }

        public static string DebugMailModeTo 
        {
            get 
            {
                var value = ConfigurationManager.AppSettings[debugEmailsToValue];

                if (value == null)
                    return null;

                return value.ToString();
            }
        }
    }
}
using MyAndromeda.Framework.Contexts;
using System;
using System.Configuration;
using System.Linq;

namespace MyAndromeda.Web.Context
{
    public class ApplicationSettings : IApplicationSettings
    {
        private readonly WorkContextWrapper workContextWrapper;

        const string DebugViewApplicationAppSetting = "MyAndromeda.Web.Settings.ShowDebugView";
        const string ProfileApplicationAppSetting = "MyAndromeda.Web.Settings.ProfileApplication";
        const string SingalRAsALoggerAppSetting = "MyAndromeda.Web.Settings.LogUsingSignalR";

        public ApplicationSettings(WorkContextWrapper workContextWrapper) 
        {
            this.workContextWrapper = workContextWrapper;
        }

        public bool DisplayDebugViews
        {
            get
            {
                //if (!this.workContextWrapper.Authorizer.Authorize(Permissions.ViewDebugMode))
                //{
                //    return false;
                //}

                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.AllKeys.Any(e => e.Equals(DebugViewApplicationAppSetting)))
                {
                    return Convert.ToBoolean(appSettings[DebugViewApplicationAppSetting]);
                }

                return false;
            }
        }

        public bool ProfileApplication
        {
            get 
            {
                //if (!this.workContextWrapper.Authorizer.Authorize(Permissions.ViewProfilingMode)) 
                //{
                //    return false;
                //}

                var appSettings = ConfigurationManager.AppSettings;

                if(appSettings.AllKeys.Any(e=> e.Equals(ProfileApplicationAppSetting)))
                {
                    return Convert.ToBoolean(appSettings[ProfileApplicationAppSetting]);
                }

                return false;
            }
        }

        public bool SignalrAsALogger
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.AllKeys.Any(e => e.Equals(SingalRAsALoggerAppSetting))) 
                {
                    return Convert.ToBoolean(appSettings[SingalRAsALoggerAppSetting]);
                }

                return false;
            }
        }
    }
}
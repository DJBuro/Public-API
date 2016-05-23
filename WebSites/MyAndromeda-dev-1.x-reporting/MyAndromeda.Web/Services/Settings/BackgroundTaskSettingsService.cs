using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using MyAndromeda.Core;
using System.Configuration;
using System.Web;

namespace MyAndromeda.Web.Services.Settings
{

    public class BackgroundTaskSettingsService : IBackgroundTaskSettingsService 
    {
        const string RunSyncronizationKey = "MyAndromeda.Tasks.RunSyncronization";
        const string RunFtpPublishingKey = "MyAndromeda.Tasks.RunFtpPublishing";
        const string RunFtpDownloadingKey = "MyAndromeda.Tasks.RunFtpDownloading";
        const string RunMenuPublishingKey = "MyAndromeda.Tasks.RunMenuPublishing";

        private NameValueCollection AppSettings 
        {
            get { return ConfigurationManager.AppSettings; }
        }

        public bool RunSyncronization
        {
            get
            {
                var value = Convert.ToBoolean(this.AppSettings[RunSyncronizationKey]);
                return value;
            }
        }

        public bool RunFtpPublishing
        {
            get
            {
                bool value = Convert.ToBoolean(this.AppSettings[RunFtpPublishingKey]);
                return value;
            }
        }

        public bool RunFtpDownloading
        {
            get
            {
                var value = Convert.ToBoolean(this.AppSettings[RunFtpDownloadingKey]);

                return value;
            }
        }

        public bool RunMenuPublishing
        {
            get
            {
                bool value = Convert.ToBoolean(this.AppSettings[RunFtpDownloadingKey]);

                return value;
            }
        }
    }

}
using System.Collections.Specialized;
using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Services.Settings
{
    public interface IBackgroundTaskSettingsService : ISingletonDependency
    {
        bool RunSyncronization { get; set; }
        bool RunFtpPublishing { get; set; }
        bool RunFtpDownloading { get; set; }
        bool RunMenuPublishing { get; set; }
        //<add key="MyAndromeda.Tasks.RunSyncronization" value="true" />
        //<add key="MyAndromeda.Tasks.RunFtpPublishing" value="true" />
        //<add key="MyAndromeda.Tasks.RunFtpDownloading" value="true" />
        //<add key="MyAndromeda.Tasks.RunMenuPublishing" value="true" />
    }

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
            set
            {
                // TODO: Implement this property setter
                throw new NotImplementedException();
            }
        }

        public bool RunFtpPublishing
        {
            get
            {
                bool value = Convert.ToBoolean(this.AppSettings[RunFtpPublishingKey]);
                return value;
            }
            set
            {
                // TODO: Implement this property setter
                throw new NotImplementedException();
            }
        }

        public bool RunFtpDownloading
        {
            get
            {
                var value = Convert.ToBoolean(this.AppSettings[RunFtpDownloadingKey]);

                return value;
            }
            set
            {
                // TODO: Implement this property setter
                throw new NotImplementedException();
            }
        }

        public bool RunMenuPublishing
        {
            get
            {
                var value = Convert.ToBoolean(this.AppSettings[RunFtpDownloadingKey]);

                return value;
            }
            set
            {
                // TODO: Implement this property setter
                throw new NotImplementedException();
            }
        }
    }
}
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
        bool RunSyncronization { get; }
        bool RunFtpPublishing { get;  }
        bool RunFtpDownloading { get; }
        bool RunMenuPublishing { get; }
        //<add key="MyAndromeda.Tasks.RunSyncronization" value="true" />
        //<add key="MyAndromeda.Tasks.RunFtpPublishing" value="true" />
        //<add key="MyAndromeda.Tasks.RunFtpDownloading" value="true" />
        //<add key="MyAndromeda.Tasks.RunMenuPublishing" value="true" />
    }

}
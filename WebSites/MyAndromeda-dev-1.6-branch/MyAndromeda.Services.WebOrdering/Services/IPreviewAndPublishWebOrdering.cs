using System.Configuration;
using System.Net.Http;
using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.WebApiClient;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Services.WebOrdering.Services
{
    public interface IPreviewAndPublishWebOrdering : IDependency
    {
        Task<HttpResponseMessage> PublishWebOrderingSettingsAsync(WebSiteConfigurations config);
        Task<HttpResponseMessage> PreviewWebOrderingSettingsAsync(WebSiteConfigurations config);
    }

}

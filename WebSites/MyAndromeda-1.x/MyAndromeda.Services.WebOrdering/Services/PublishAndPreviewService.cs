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
        Task<bool> PreviewWebOrderingSettingsAsync(WebSiteConfigurations config);
    }

    public class PreviewPublishTheme : IPreviewAndPublishWebOrdering
    {
        private readonly IMyAndromedaLogger logger;

        public PreviewPublishTheme(IMyAndromedaLogger logger)
        {
            this.logger = logger;
        }

        public async Task<HttpResponseMessage> PublishWebOrderingSettingsAsync(WebSiteConfigurations config)
        {
            bool result = false;
            HttpResponseMessage response = null;

            Uri serviceUrl = new Uri(ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"]);
            using (var client = ClientHelper.GetANewJsonClient(serviceUrl))
            {
                
                this.logger.Debug("Await response");
                response = await client.PostAsJsonAsync(serviceUrl, config);
                this.logger.Debug("response fetched and is: " + response.IsSuccessStatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    string msg = response.Content.ReadAsStringAsync().Result;
                    this.logger.Error(msg);
                }

                result = response.IsSuccessStatusCode;
            }


            //return result;
            return response;
        }

        public async Task<bool> PreviewWebOrderingSettingsAsync(WebSiteConfigurations config)
        {
            bool result = false;

            //config.SiteDetails = config.OldPreviewDomainName;
            //config.LiveDomainName = config.OldPreviewDomainName;

            Uri serviceUrl = new Uri(ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"]);
            using (var client = ClientHelper.GetANewJsonClient(serviceUrl))
            {
                
                this.logger.Debug("Await response");
                var response = await client.PostAsJsonAsync(serviceUrl, config);
                this.logger.Debug("response fetched and is: " + response.IsSuccessStatusCode);
                result = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    string msg = response.Content.ReadAsStringAsync().Result;
                    this.logger.Error(msg);
                }
            }


            return result;
        }
    }
}

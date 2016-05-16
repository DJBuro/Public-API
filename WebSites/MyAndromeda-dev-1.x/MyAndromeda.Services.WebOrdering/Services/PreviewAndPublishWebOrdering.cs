using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using MyAndromeda.Core;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.WebApiClient;
using AndroAdminDataAccess.Domain.WebOrderingSetup;

namespace MyAndromeda.Services.WebOrdering.Services
{

    public class PreviewAndPublishWebOrdering : IPreviewAndPublishWebOrdering
    {
        private readonly IMyAndromedaLogger logger;

        public PreviewAndPublishWebOrdering(IMyAndromedaLogger logger)
        {
            this.logger = logger;
        }

        public async Task<HttpResponseMessage> PublishWebOrderingSettingsAsync(WebSiteConfigurations config)
        {
            bool result = false;
            HttpResponseMessage response = null;

            var serviceUrl = new Uri(ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"]);
            using (var client = ClientHelper.GetANewJsonClient(serviceUrl))
            {
                
                this.logger.Debug(message: "Await response");
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

        public async Task<HttpResponseMessage> PreviewWebOrderingSettingsAsync(WebSiteConfigurations config)
        {
            bool result = false;
            HttpResponseMessage response = null;
            //config.SiteDetails = config.OldPreviewDomainName;
            //config.LiveDomainName = config.OldPreviewDomainName;

            Uri serviceUrl = new Uri(ConfigurationManager.AppSettings["MyAndromeda.WebOrderingWebSite.CreateService"]);
            using (var client = ClientHelper.GetANewJsonClient(serviceUrl))
            {
                
                this.logger.Debug(message: "Await response");
                response = await client.PostAsJsonAsync(serviceUrl, config);
                this.logger.Debug("response fetched and is: " + response.IsSuccessStatusCode);
                result = response.IsSuccessStatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    string msg = response.Content.ReadAsStringAsync().Result;
                    this.logger.Error(msg);
                }
            }


            return response;
        }
    }

}
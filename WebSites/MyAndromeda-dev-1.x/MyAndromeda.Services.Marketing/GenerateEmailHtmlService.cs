using System;
using System.Linq;
using MyAndromeda.Data.DataAccess.WebOrdering;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.SendGridService.Models;
using MyAndromeda.WebApiClient;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Postal;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace MyAndromeda.Services.Marketing
{
    public class GenerateEmailHtmlService : IGenerateEmailHtmlService 
    {
        private readonly IWebOrderingWebSiteDataService websiteDataService;
        private readonly IStoreDataService storeDataService;
        private readonly IWebApiClientContext webApiClientContext;

        public GenerateEmailHtmlService(IWebOrderingWebSiteDataService websiteDataService, IStoreDataService storeDataService, IWebApiClientContext webApiClientContext)
        {
            this.webApiClientContext = webApiClientContext;
            this.storeDataService = storeDataService;
            this.websiteDataService = websiteDataService;
        }

        public string HtmlForWebApi(MarketingEventCampaign campaign)
        {
            return this.GenerateEmailMessage(campaign);
        }

        public async Task<string> HtmlForWebJob(MarketingEventCampaign campaign)
        {
            string result = string.Empty;
            //call web services ; 
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(webApiClientContext.BaseAddress);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var route = string.Format("/marketing/{0}/marketing/html", campaign.AndromedaSiteId);

                HttpResponseMessage response = await
                    client.PostAsJsonAsync(route, campaign);

                if (!response.IsSuccessStatusCode)
                {
                    string message = string.Format("Problem calling: {0}", route);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    throw new WebException(message, new Exception(responseMessage));
                }

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        private string GenerateEmailMessage(MarketingEventCampaign campaign)
        {
            var store = this.storeDataService.Table.Where(e => e.AndromedaSiteId == campaign.AndromedaSiteId).First();
            var outerTemplate = this.GenerateOuterTemplate(store);

            return outerTemplate.Replace("<% body %>", campaign.EmailTemplate);
        }

        private string GenerateOuterTemplate(Store store)
        {
            var website = this.websiteDataService.Table.Where(e => e.ACSApplication
                                                                    .ACSApplicationSites
                                                                    .Any(app => app.SiteId == store.Id))
                              .First();
            //generate the template 
            var message = new MarketingEmailMessage()
            {
                Store = store,
                WebsiteStuff = website
            };

            var postalEmailService = new EmailService();
            var output = postalEmailService.CreateMailMessage(message);

            return output.Body;
        }
    }
}
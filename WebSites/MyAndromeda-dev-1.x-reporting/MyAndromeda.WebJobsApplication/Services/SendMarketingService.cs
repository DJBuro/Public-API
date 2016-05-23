using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;
using MyAndromeda.Services.Marketing;
using MyAndromeda.Services.Marketing.Models;

namespace MyAndromeda.WebJobs.EventMarketing.Services
{
    public class SendMarketingService : ISendMarketingService 
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IMarketingTemplateDataService marketingTemplateDataService;
        private readonly IMarketingOverlord overlord;
        private readonly IGenerateEmailHtmlService generatehtmlTemplateService;
        private readonly IRecipientListForEventMarketingService recipientListForEventMarketingService;

        public SendMarketingService(IMyAndromedaLogger logger, IMarketingTemplateDataService marketingTemplateDataService, IMarketingOverlord overlord, IGenerateEmailHtmlService generatehtmlTemplateService, IRecipientListForEventMarketingService recipientListForEventMarketingService)
        {
            this.recipientListForEventMarketingService = recipientListForEventMarketingService;
            this.generatehtmlTemplateService = generatehtmlTemplateService;
            this.overlord = overlord;
            this.marketingTemplateDataService = marketingTemplateDataService;
            this.logger = logger;
        }

        public async Task<MarketingRecipientListMessage> SendAsyc(MarketingStoreEventQueueMessage message,
            bool addMatt = true,
            bool sendTheEmail = true)
        {
            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                                   .FirstOrDefaultAsync(e =>
                                                            e.AndromedaSiteId == message.AndromedaSiteId &&
                                                            e.TemplateName == message.MarketingCampaignType);

            this.logger.Debug("Create recipient list");

            var people = await this
                .recipientListForEventMarketingService
                .GetRecipients(entity);

            if (addMatt) 
            {
                var me = new SendGridService.MarketingApi.Models.Recipients.Person()
                {
                    Email = "matthew_green@androtech.com",
                    Name = "Matt",
                    Title = "Test",

                };
                dynamic facade = me;
                facade.loyaltyPoints = "2000000";
                facade.loyaltyPointsValue = "1 million dollars";
                people.Add(me);
            }

            this.logger.Debug("Get store recipient list");

            if (people.Count == 0) 
            {
                this.logger.Debug("Finishing Job. There are no recipients for store: {0}" + message.MarketingCampaignType, message.AndromedaSiteId);
                return null;
            }

            //set the list 
            bool added = await this.overlord.AddPeopleToRecipientListAsync(entity, people);

            if (!added)
            {
                throw new Exception("Adding people failed");
            }

            this.logger.Debug("Create template");

            //create template + from 
            GetResponseTemplateModel addTemplate;
            try
            {
                //todo - call webservice to grab the template. View wont be found here. 
                var html = await this.generatehtmlTemplateService.HtmlForWebJob(entity);

                addTemplate = await this.overlord.UpdateSendGridTemplate(entity, html);
            }
            catch (Exception ex)
            {
                this.logger.Error("could not create template");
                this.logger.Error(ex);
                throw ex;
            }

            this.logger.Debug("Get 'created' sendgrid template");

            var k = await this.overlord.GetMarketingInfoAsync(entity, addTemplate.Name);

            this.logger.Debug("add some categories to the template");

            var categories = new[] { "Test", "Matt's Test" };
            await this.overlord.AddCategoryAsync(k.Template.Name, categories);

            try
            {
                this.logger.Debug("add the recipient list");

                bool addedRecipients = await this.overlord.AddRecipientListsToTemplateAsync(entity, k.Template.Name);

                if (!addedRecipients) 
                {
                    throw new Exception("Recipient list was not added.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
                throw;
            }

            if (sendTheEmail) 
            { 
                bool send = false;
                
                try
                {
                    this.logger.Debug("send email");

                    send = await this.overlord.SendCampaignAsync(entity, k.Template.Name);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex.Message);
                    throw;
                }

                if (!send)
                {
                    this.logger.Error("I did not work");
                    throw new Exception("Sending failed");
                }
            }

            var outputMessageObject = new MarketingRecipientListMessage()
            {
                People = people,
                Template = addTemplate,
                Categories = categories,
                MarketingCampaignType = message.MarketingCampaignType,
                AndromedaSiteId = message.AndromedaSiteId
            };

            return outputMessageObject;
        }

        public void AddTestUsers() 
        {
            //var testPeople = new List<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>();
            //dynamic person1 = new SendGridService.MarketingApi.Models.Recipients.Person()
            //{
            //    Email = "matthew.green@androtech.com",
            //    Name = "Matthew Green"
            //};
            //dynamic person2 = new SendGridService.MarketingApi.Models.Recipients.Person()
            //{
            //    Email = "matthew_green@androtech.com",
            //    Name = "Matthew Green",
            //};
            //person1.SaySomething = "hi";
            //person2.SaySomething = "hi from person 2";
            //testPeople.Add(person1);
            //testPeople.Add(person2);
        }
    }
}
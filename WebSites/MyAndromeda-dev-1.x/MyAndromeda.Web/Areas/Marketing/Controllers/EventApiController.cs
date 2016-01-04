using System.Security;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService;
using MyAndromeda.SendGridService.MarketingApi.Models;
using MyAndromeda.SendGridService.MarketingEmails;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Validation;
using MyAndromeda.SendGridService.Models;
using MyAndromeda.Services.Marketing;
using MyAndromeda.Web.Areas.Marketing.Models;
using Postal;
using MyAndromeda.SendGridService.MarketingApi;
using MyAndromeda.SendGridService.MarketingApi.Models.Contact;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;
using System.Net.Http;
using System.Text;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class EventApiController : ApiController
    {
        private readonly IMarketingTemplateDataService marketingTemplateDataService;
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;

        private readonly ICurrentSite currentSite;
        private readonly IPreviewEmailCampaign previewEmailCampaign;
        private readonly ISenderAddressService senderAddressService;
        private readonly ITemplateService templateService;

        //private readonly IMarketingOverlord overlord;
        private readonly IQueueMarketingEventEmailService queueMarketingEmailEventService;

        private readonly IAuthorizer authorizer;

        public EventApiController(IMyAndromedaLogger logger,
            INotifier notifier,
            ICurrentSite currentSite,
            IMarketingTemplateDataService marketingTemplateDataService,
            IPreviewEmailCampaign previewEmailCampaign,
            ISenderAddressService senderAddressService,
            ITemplateService templateService,
            IQueueMarketingEventEmailService queueMarketingEmailEventService,
            IAuthorizer authorizer)
        { 
            this.authorizer = authorizer;
            this.queueMarketingEmailEventService = queueMarketingEmailEventService;
            this.templateService = templateService;
            this.senderAddressService = senderAddressService;
            this.previewEmailCampaign = previewEmailCampaign;
            this.currentSite = currentSite;
            this.logger = logger;
            this.notifier = notifier;
            this.marketingTemplateDataService = marketingTemplateDataService;
        }

        private async Task UpdateSendGridContactInfo(MarketingContact entity) 
        {
            try
            {
                SendGridMessage addOrUpdateResult = null;

                var model = entity.Convert();
                var result = await this.senderAddressService.GetAsync(new GetSenderAddressModel()
                {
                    Identity = model.Identity
                });

                if (result == null)
                {
                    addOrUpdateResult = await this.senderAddressService.AddAsync(entity.Convert());
                }
                else
                {
                    addOrUpdateResult = await this.senderAddressService.EditAsync(entity.Convert());
                }

                if (!addOrUpdateResult.IsSuccessful) 
                {
                    this.logger.Error("Could not update sendgrid");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Updating sendgrid problem.");
                this.logger.Error(ex);
                throw;
            }
        }

        private async Task<GetRequestTemplateModel> UpdateSendGridTemplate(MarketingEventCampaign entity) 
        {

            var contactEntity = await this.marketingTemplateDataService.MarketingContacts
                .FirstOrDefaultAsync(e => e.AndromedaSiteId == entity.AndromedaSiteId);

            var template = this.GenerateEmailMessage(entity);
            GetRequestTemplateModel model = entity.ConvertToGetTemplateModel();
            try
            {
                SendGridMessage addOrUpdateResult = null;

                var result = await this.templateService.GetAsync(model);

                if (result == null)
                {
                    var templateModel = entity.ConvertToAddModel(contactEntity, model.Name, template);
                    addOrUpdateResult = await this.templateService.AddAsync(templateModel);
                }
                else 
                {
                    var templateModel = entity.ConvertToEditModel(contactEntity, model.Name, template);
                    addOrUpdateResult = await this.templateService.EditAsync(templateModel);
                }

                if (!addOrUpdateResult.IsSuccessful)
                {
                    this.logger.Error("Could not update sendgrid");
                }
            }
            catch (Exception e)
            {
                this.logger.Error("updating sendgrid problem.");
                throw;
            }

            return model;
        }

        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/contact")]
        public async Task<MarketingContact> GetContactModel(int andromedaSiteId)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingContacts
                .FirstOrDefaultAsync(e => e.AndromedaSiteId == andromedaSiteId);

            if (entity == null) 
            {
                entity = new MarketingContact();
                var address = await 
                    this.marketingTemplateDataService.Addresses
                        .Include(e=> e.Country)
                        .FirstOrDefaultAsync(e => e.Stores.Any(s => s.AndromedaSiteId == andromedaSiteId));

                entity.Name = string.Empty;
                entity.From = string.Empty;
                entity.ReplyTo = string.Empty;

                entity.Address = string.Format("{0}, {1}", address.RoadNum, address.RoadName);
                entity.AndromedaSiteId = andromedaSiteId;
                entity.City = address.Town;
                entity.Country = address.Country.CountryName;
                entity.County = address.County;
                entity.PostCode = address.PostCode;

                this.marketingTemplateDataService.MarketingContacts.Add(entity);

                try
                {
                    await this.marketingTemplateDataService.SaveAsync();
                }
                catch (DbEntityValidationException dbExecption)
                {
                    this.logger.Error("Failed to create contact record");
                    throw;
                }
            }

            //
            return entity;
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/contact")]
        public async Task<MarketingContact> PostContactModel([FromUri]int andromedaSiteId, [FromBody]MarketingContact model)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingContacts
                .FirstOrDefaultAsync(e => e.AndromedaSiteId == andromedaSiteId);

            entity.Address = model.Address;
            entity.City = model.City;
            entity.Country = model.Country;
            entity.County = model.County;
            entity.From = model.From;
            entity.Name = model.Name;
            entity.PostCode = model.PostCode;
            entity.ReplyTo = model.ReplyTo;

            try
            {
                await this.marketingTemplateDataService.SaveAsync();
            }
            catch (DbEntityValidationException dbExecption)
            {
                this.logger.Error("Failed to create contact record");
                this.logger.Error(dbExecption);

                this.notifier.Error("The record could not be saved.");

                throw;
            }
            try
            {
                await this.UpdateSendGridContactInfo(entity);
            }
            catch (Exception ex)
            {
                this.notifier.Error("The cloud could not be updated.");
                this.logger.Error(ex);

                throw;
            }

            this.notifier.Success("Contact data updated successfully.", true);


            return entity;
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/saveevent")]
        public async Task<MarketingEventCampaign> SaveTemplate([FromUri]int andromedaSiteId, [FromBody]MarketingEventCampaign model)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService
                .MarketingEventCampaigns
                .FirstOrDefaultAsync(e =>
                    e.AndromedaSiteId == andromedaSiteId && e.TemplateName == model.TemplateName);

            if (entity == null)
            {
                this.marketingTemplateDataService.MarketingEventCampaigns.Add(model);
                entity = model;
            }
            else
            {
                entity.EmailTemplate = model.EmailTemplate;
                entity.EnableEmail = model.EnableEmail;
                entity.EnableSms = model.EnableSms;
                entity.SmsTemplate = model.SmsTemplate;
                entity.Subject = model.Subject;
                entity.TemplateName = model.TemplateName;

                entity.Preview = model.Preview;
                entity.PreviewCreated = model.PreviewCreated;
            }

            //save record
            await this.marketingTemplateDataService.SaveAsync();

            //update send-grid -- postponed till later
            //await this.UpdateSendGridTemplate(entity);

            this.notifier.Success("Marketing event updated successfully.", true);

            return entity;
        }


        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/noorders")]
        public async Task<MarketingEventCampaign> GetNoOrdersTemplate([FromUri]int andromedaSiteId)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                .FirstOrDefaultAsync(e => 
                    e.AndromedaSiteId == andromedaSiteId && 
                    e.TemplateName == MarketingEventTypes.NoOrders);

            return entity ?? new MarketingEventCampaign() 
            {
                AndromedaSiteId = andromedaSiteId,
                TemplateName = MarketingEventTypes.NoOrders,
                EmailTemplate = string.Empty,
                SmsTemplate = string.Empty,
                Subject = string.Empty,
                EnableEmail = false,
                EnableSms = false
            };
        }

        
        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/oneweek")]
        public async Task<MarketingEventCampaign> GetCurrentCustomerTemplate([FromUri]int andromedaSiteId)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                .FirstOrDefaultAsync(e =>
                    e.AndromedaSiteId == andromedaSiteId &&
                    e.TemplateName == MarketingEventTypes.OneWeekNoOrders);

            return entity ?? new MarketingEventCampaign()
            {
                AndromedaSiteId = andromedaSiteId,
                TemplateName = MarketingEventTypes.OneWeekNoOrders,
                EmailTemplate = string.Empty,
                SmsTemplate = string.Empty,
                Subject = string.Empty,
                EnableEmail = false,
                EnableSms = false
            };
        }

        
        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/onemonth")]
        public async Task<MarketingEventCampaign> GetLazyCustomerTemplate([FromUri]int andromedaSiteId)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                .FirstOrDefaultAsync(e =>
                    e.AndromedaSiteId == andromedaSiteId &&
                    e.TemplateName == MarketingEventTypes.OneMonthNoOrders);

            return entity ?? new MarketingEventCampaign()
            {
                AndromedaSiteId = andromedaSiteId,
                TemplateName = MarketingEventTypes.OneMonthNoOrders,
                EmailTemplate = string.Empty,
                SmsTemplate = string.Empty,
                Subject = string.Empty,
                EnableEmail = false,
                EnableSms = false
            };
        }

        
        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/threemonth")]
        public async Task<MarketingEventCampaign> GetLapsedCustomerTemplate([FromUri]int andromedaSiteId)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                .FirstOrDefaultAsync(e =>
                    e.AndromedaSiteId == andromedaSiteId &&
                    e.TemplateName == MarketingEventTypes.ThreeMonthNoOrders);

            return entity ?? new MarketingEventCampaign()
            {
                AndromedaSiteId = andromedaSiteId,
                TemplateName = MarketingEventTypes.ThreeMonthNoOrders,
                EmailTemplate = string.Empty,
                SmsTemplate = string.Empty,
                Subject = string.Empty,
                EnableEmail = false,
                EnableSms = false
            };
        }


        [HttpGet]
        [Route("marketing/{andromedaSiteId}/marketing/test")]
        public async Task<MarketingEventCampaign> GetTestTemplate([FromUri]int andromedaSiteId) 
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var entity = await this.marketingTemplateDataService.MarketingEventCampaigns
                .FirstOrDefaultAsync(e =>
                    e.AndromedaSiteId == andromedaSiteId &&
                    e.TemplateName == MarketingEventTypes.TestEventCampaign);

            return entity ?? new MarketingEventCampaign()
            {
                AndromedaSiteId = andromedaSiteId,
                TemplateName = MarketingEventTypes.TestEventCampaign,
                EmailTemplate = string.Empty,
                SmsTemplate = string.Empty,
                Subject = string.Empty,
                EnableEmail = false,
                EnableSms = false
            };
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/preview")]
        public async Task<MarketingEventCampaign> Preview(
            [FromUri]int andromedaSiteId, 
            [FromBody]PreviewMarketing model)
        {
            if (!authorizer.Authorize(UserPermissions.ChangeEventMarketing))
            {
                throw new SecurityException("No permissions");
            }

            var output = this.GenerateOuterTemplate();

            var emails = model.Preview.To
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e=> e.Trim())
                .ToArray()
                ;

            var result = await this.previewEmailCampaign.SendAsync(output, emails, model.Model, model.Preview.Send);

            model.Model.Preview = result;

            return model.Model;
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/sendnow")]
        public async Task<bool> SendCampaign([FromBody]MarketingEventCampaign model) 
        {
            this.notifier.Success("Adding model to the campaign queue");

            try
            {
                var queueName = await this.queueMarketingEmailEventService.AddToMarketingQueueAsync(model.AndromedaSiteId, model.TemplateName);
                
                //expecting null
                if (queueName == null) 
                {
                    this.notifier.Success("Already queued.");
                }
                else 
                {
                    this.notifier.Success("Added model to the campaign queue: " + queueName);
                }
                
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed sending the marketing event to the queue.");
                this.logger.Error(ex);

                throw;
            } 

            
            return true;
        }

        [HttpPost]
        [Route("marketing/{andromedaSiteId}/marketing/html")]
        public async Task<HttpResponseMessage> Html([FromBody]MarketingEventCampaign model) 
        {
            string content = this.GenerateEmailMessage(model);
            //var result = base.Content(HttpStatusCode.OK, content);
            //return result;
            
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    content,
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }

        private string GenerateEmailMessage(MarketingEventCampaign campaign) 
        {
            var outerTemplate = this.GenerateOuterTemplate();
            
            return outerTemplate.Replace("<% body %>", campaign.EmailTemplate);
        }

        private string GenerateOuterTemplate() 
        {
            //generate the template 
            var message = new MarketingEmailMessage()
            {
                Store = this.currentSite.Store,
                WebsiteStuff = this.currentSite.AndroWebOrderingSites.First()
            };

            var postalEmailService = new EmailService();
            var output = postalEmailService.CreateMailMessage(message);

            return output.Body;
        }
    }
}
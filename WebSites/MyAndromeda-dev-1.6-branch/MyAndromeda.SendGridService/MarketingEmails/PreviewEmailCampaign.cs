using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyAndromeda.Logging;

namespace MyAndromeda.SendGridService.MarketingEmails
{
    public class PreviewEmailCampaign : MyAndromeda.SendGridService.MarketingEmails.IPreviewEmailCampaign
    {
        private readonly ISendGridEmailSettings sendGridEmailSettings;
        private readonly IMyAndromedaLogger logger;

        public PreviewEmailCampaign(ISendGridEmailSettings sendGridEmailSettings, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.sendGridEmailSettings = sendGridEmailSettings;
        }

        public async Task<string> SendAsync(
            string outerTemplate, 
            string[] toAddresses, 
            MyAndromeda.Data.Model.MyAndromeda.MarketingEventCampaign model = null,
            bool send = true)
        {
            try
            {
                var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
                var client = new SendGrid.Web(credentials);

                SendGrid.SendGridMessage myMessage = new SendGrid.SendGridMessage();

                myMessage.AddTo(toAddresses);
                myMessage.AddBcc("matthew_green@androtech.com");
                
                myMessage.From = new System.Net.Mail.MailAddress("matthew.green@androtech.com", "Matt's pizza");

                myMessage.EnableTemplate(outerTemplate);

                myMessage.SetCategories(new[] { 
                    Categories.Marketing
                });


                if (model == null)
                {
                    myMessage.Html = "<div><strong>-Name- Hello World! -Name- Wants to: -WantsTo-</strong><a href='-unSubscribe-'>un-subscribe</a></div>";
                    myMessage.Subject = "Test multiple";
                }
                else 
                {
                    myMessage.Html = model.EmailTemplate;
                    myMessage.Subject = model.Subject;
                }

                myMessage.EnableUnsubscribe("-unSubscribe-");
                myMessage.EnableOpenTracking();
                
                myMessage.AddSubstitution("-Name-", new List<string> { "Matt", "Matt2", });
                myMessage.AddSubstitution("-WantsTo-", new List<string> { "Eat", "Escape" });

                if (send) 
                { 
                    await client.DeliverAsync(myMessage);
                }

                return outerTemplate.Replace("<% body %>", model.EmailTemplate);
            }
            catch (Exception e)
            {

                this.logger.Error(e);
                throw;
            }

        }
    }
}

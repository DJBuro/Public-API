using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Framework.Tokens;
using MyAndromeda.Logging;

namespace MyAndromeda.SendGridService.MarketingEmails
{
    public class EmailCampaign : MyAndromeda.SendGridService.MarketingEmails.IEmailCampaign
    {
        private readonly ISendGridEmailSettings sendGridEmailSettings;
        private readonly ITokenProvider tokenProvider;
        private readonly IMyAndromedaLogger logger;

        public EmailCampaign(ISendGridEmailSettings sendGridEmailSettings, ITokenProvider tokenProvider, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.tokenProvider = tokenProvider;
            this.sendGridEmailSettings = sendGridEmailSettings;
        }

        public async Task Send()
        {
            try
            {
                var credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
                var client = new SendGrid.Web(credentials);

                SendGrid.SendGridMessage myMessage = new SendGrid.SendGridMessage();

                myMessage.AddTo("matthew.green@androtech.com");

                myMessage.AddBcc("matthew.green@androtech.com");
                myMessage.AddBcc("barmy_matt@hotmail.com");


                myMessage.From = new System.Net.Mail.MailAddress("matthew.green@androtech.com");

                myMessage.EnableTemplate("<% body %>");

                myMessage.SetCategories(new[] { "Test" });

                myMessage.Html = "<div><strong>-Name- Hello World! -Name- Wants to: -WantsTo-</strong><a href='-unSubscribe-'>un-subscribe</a></div>";
                myMessage.EnableUnsubscribe("-unSubscribe-");
                myMessage.Subject = "Test multiple";
                myMessage.AddSubstitution("-Name-", new List<string> { "Matt", "Rob", });
                myMessage.AddSubstitution("-WantsTo-", new List<string> { "Eat", "Escape" });

                await client.DeliverAsync(myMessage);
            }
            catch (Exception e)
            {

                this.logger.Error(e);
                throw;
            }

        }
    }
}

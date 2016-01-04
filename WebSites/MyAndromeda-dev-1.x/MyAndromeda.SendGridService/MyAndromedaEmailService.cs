using System.Collections.Generic;
using System.Net.Mail;
using MyAndromeda.Logging;
using Postal;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using MyAndromeda.SendGridService.Events;

namespace MyAndromeda.SendGridService
{
    public class MyAndromedaEmailService : IMyAndromedaTransactionalEmailService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ICreatedTransactinoalEmailEvent events;

        
        private readonly SendGrid.Web client;
        
        private readonly EmailService postalEmailService;
        private readonly NetworkCredential credentials;

        private readonly string[] categoryList = new[] { Categories.Transactional };
        
        
        public MyAndromedaEmailService(IMyAndromedaLogger logger,
            ISendGridEmailSettings sendGridEmailSettings,
            ICreatedTransactinoalEmailEvent events) 
        {
            this.events = events;
            this.logger = logger;
            this.postalEmailService = new EmailService();

            this.credentials = new NetworkCredential(sendGridEmailSettings.UserName, sendGridEmailSettings.Password);
            this.client = new SendGrid.Web(credentials);
        }

        public void Send(Email email)
        {
            try
            {
                var task = this.SendAsync(email);
                task.Wait();
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }
        }

        private SendGrid.SendGridMessage CreateMeAMessage(Email email, IEnumerable<string> categories) 
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            SendGrid.SendGridMessage myMessage = new SendGrid.SendGridMessage();

            myMessage.SetMyAndromedaDefaults();
            myMessage.Copy(emailMessage);
            myMessage.From = emailMessage.From;
            myMessage.ReplyTo = emailMessage.ReplyToList.ToArray();

            myMessage.SetCategories(categories);

            return myMessage;
        }

        public async Task SendAsync(Email email, int andromedaSiteId, Guid orderId, Guid? customerId)
        {
            try
            {
                var myMessage = this.CreateMeAMessage(email, categoryList);
                var args = new Dictionary<string, string>();

                var emailEntity = await this.events.CreatedTransactionalOrderEmailAsync(myMessage, categoryList,
                        andromedaSiteId: andromedaSiteId,
                        orderId: orderId,
                        customerId: customerId);

                args.Add("emailId", emailEntity.Id.ToString());
                args.Add("andromedaSiteId", andromedaSiteId.ToString());
                args.Add("orderId", orderId.ToString());

                myMessage.AddUniqueArgs(args);

                await client.DeliverAsync(myMessage);
            }
            catch (Exception e)
            {
                this.logger.Error("Error sending a email async");
                this.logger.Error(e);
                throw;
            }
        }

        public async Task SendAsync(Email email)
        {
            try
            {
                var myMessage = this.CreateMeAMessage(email, categoryList);

                await client.DeliverAsync(myMessage);
            }
            catch (Exception e)
            {
                this.logger.Error("Error sending a email async");
                this.logger.Error(e);
                throw;
            }
            
        }

        public MailMessage CreateMailMessage(Email email)
        {
            var emailMessage = postalEmailService.CreateMailMessage(email);

            return emailMessage;
        }
    }

    public static class Extensions 
    {

        public static void SetMyAndromedaDefaults(this SendGrid.SendGridMessage email) 
        {
            email.EnableClickTracking();
            email.EnableGravatar();
            email.EnableOpenTracking();
        }

        public static void Copy(this SendGrid.SendGridMessage email, MailMessage message) 
        {
            //email.To = message.To.ToArray();
            foreach (var to in message.To) 
            {
                email.AddTo(to.Address);
            }
            foreach (var to in message.CC) 
            {
                email.AddCc(to.Address);
            }
            foreach (var to in message.Bcc) 
            {
                email.AddBcc(to.Address);
            }

            email.Subject = message.Subject;
            email.Html = message.Body;
        } 
    }
}

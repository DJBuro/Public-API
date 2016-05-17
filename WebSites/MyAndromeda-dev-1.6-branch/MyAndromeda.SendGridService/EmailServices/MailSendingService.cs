using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.Domain.Marketing;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Messaging;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Tokens;
using System.Configuration;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataWarehouse.Domain.Marketing;

namespace MyAndromeda.SendGridService.EmailServices
{
    public class MailSendingService : IMailSendingService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;
        private readonly IMailSender mailSender;
        private readonly ITokenService tokenService;
        
        private readonly WorkContextWrapper workContextWrapper;

        public MailSendingService(
            WorkContextWrapper workContextWrapper,
            IMyAndromedaLogger logger,
            ITokenService tokenService,
            IMailSender mailSender,
            INotifier notifier)
        { 
            this.workContextWrapper = workContextWrapper;
            this.notifier = notifier;
            this.tokenService = tokenService;
            this.logger = logger;
            this.mailSender = mailSender;
        }

        public void SendPreviewEmail(EmailCampaign model, EmailSettings settings, string emailTo)
        {
            this.logger.Debug("Sending a preview email for campaign {0}: " + emailTo, model.Id);

            var slitToWith = new[] { ',', ';' };
            var too = emailTo.Split(slitToWith, StringSplitOptions.RemoveEmptyEntries);

            logger.Info("Grabbing customer information for chain: {0}", model.ChainId);

            var users = new CustomerModel[] { 
                new CustomerModel() {
                    Title = "Mr",
                    FirstName = "Joe Bloggs"
                }
            };
            
            //var users = this.dataAccessFactory.CustomerDataAccess.ListByChain(model.ChainId).ToArray();

            //if (users.Length == 0) 
            //{
            //    this.notifier.Error("There is no customer data to test with! ... Making up a person ...");
            //    users = new[] { 
            //        new Customer(){
            //            Id = 0,
            //            FirstName = "Joe",
            //            Surname = "Bloggs",
            //            Title = "Mr"
            //        }
            //    };
            //    //throw new ArgumentNullException("Users");
            //}

            //var testPool = new Customer[] { users.Random() };
            
            this.SendEmails(model, settings, users, too);
        }

        public void SendCampaignEmail(
            Data.Domain.Marketing.EmailCampaign model, 
            Data.Domain.Marketing.EmailSettings settings = null)
        {
            this.logger.Info("Sending campaign email: {0}", model.Id);

            //var users = this.dataAccessFactory.CustomerDataAccess.ListByChain(model.ChainId);

            //this.SendEmails(model, settings, users);
        }

        public void SendEmails(Data.Domain.Marketing.EmailCampaign model, Data.Domain.Marketing.EmailSettings settings, IEnumerable<CustomerModel> users, string[] overideTo = null)
        {
            var tokenContext = new TokenContext()
            {
                Message = model.EmailTemplate
            };

            if (this.workContextWrapper.DebugMode) 
            {
                overideTo = new[] { DebugMailModeTo };
            }

            var messages = this.tokenService.ProcessTokens(tokenContext, users);

            foreach (var message in messages)
            {
                //could use the guid to track the completion of the email
                this.mailSender.Send(overideTo ?? 
                    new[] { 
                        message.Model.ContactDetails
                            .Single(e=> e.ContactType == MyAndromedaDataAccess.Domain.DataWarehouse.ContactType.Email).Value 
                    }, model.Subject, message.Output, settings, Guid.NewGuid() 
                    );
            }
        }

        private static readonly string debugEmailsToValue = "StagingMailModeToValue";

        public static string DebugMailModeTo
        {
            get
            {
                var value = ConfigurationManager.AppSettings[debugEmailsToValue];

                if (value == null)
                    return null;

                return value.ToString();
            }
        }

        /// <summary>
        /// Logs the sending email.
        /// </summary>
        /// <param name="to">To.</param>
        private void LogSendingEmail(string[] to)
        {
            this.logger.Debug("Marketing email request going to {0} people.", to.Length);
        }
    }
}

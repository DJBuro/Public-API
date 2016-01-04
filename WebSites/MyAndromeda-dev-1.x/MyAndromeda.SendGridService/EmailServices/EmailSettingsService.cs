using System;
using System.Configuration;
using System.Net.Configuration;
using MyAndromeda.Data.DataAccess.MyAndromeda.Email;
using MyAndromeda.Data.Domain.Marketing;

namespace MyAndromeda.SendGridService.EmailServices
{
    public class EmailSettingsService : IEmailSettingsService
    {
        private readonly IEmailCampaignDataAccess emailCampaignDataAccess;

        public EmailSettingsService(IEmailCampaignDataAccess dataFactory)
        {
            this.emailCampaignDataAccess = dataFactory;
        }

        public EmailSettings GetBestSettings(int chainId)
        {
            EmailSettings settings = null;
            if (chainId > 0)
            {
                settings = this.emailCampaignDataAccess.GetEmailSettingsByChainId(chainId);
            }
            if (settings == null)
            {
                settings = LoadWebConfigSettings();
            }

            return settings;
        }

        public void Create(EmailSettings settings)
        {
            emailCampaignDataAccess.CreateEmailSettings(settings);
        }

        public EmailSettings Get(int id)
        {
            return emailCampaignDataAccess.GetEmailSettings(id);
        }

        public void Destroy(int id)
        {
            emailCampaignDataAccess.DestroyEmailSettings(id);
        }

        public void Update(EmailSettings settings)
        {
            emailCampaignDataAccess.SaveSettings(settings);
        }

        private EmailSettings LoadWebConfigSettings()
        {
            var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (section == null)
            {
                throw new ArgumentException("There are no SMTP settings");
            }

            var emailSettings = new MyAndromeda.Data.Domain.Marketing.EmailSettings()
            {
                Host = section.Network.Host,
                PickupFolder = section.SpecifiedPickupDirectory == null ? null : section.SpecifiedPickupDirectory.PickupDirectoryLocation,
                Port = section.Network.Port,
                From = section.From,
                UserName = section.Network.UserName,
                //Password = section.Network.Password
            };

            return emailSettings;
        }
    }
}
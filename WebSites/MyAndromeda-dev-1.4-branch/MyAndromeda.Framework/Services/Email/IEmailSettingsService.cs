using System;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using MyAndromeda.Core;
using MyAndromedaDataAccess.DataAccess.MyAndromeda.Email;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Framework.Services.Email
{
    public interface IEmailSettingsService : IDependency
    {
        /// <summary>
        /// Gets the best email settings available - either stored by chain or default web.config.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        EmailSettings GetBestSettings(int chainId);

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        EmailSettings Get(int id);

        /// <summary>
        /// Destroys the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Destroy(int id);

        /// <summary>
        /// Updates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void Update(EmailSettings settings);

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Create(EmailSettings settings);
    }

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

            var emailSettings = new MyAndromedaDataAccess.Domain.Marketing.EmailSettings()
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

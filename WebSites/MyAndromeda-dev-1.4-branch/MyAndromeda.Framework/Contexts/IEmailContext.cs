using System;
using System.Configuration;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Framework.Services.Email;
using MyAndromedaDataAccess;
using MyAndromedaDataAccess.Domain.Marketing;
using System.Net.Configuration;

namespace MyAndromeda.Framework.Contexts
{
    public interface IEmailContext : IDependency
    {
        EmailSettings EmailSettings { get; }
    }

    public class EmailContext : IEmailContext 
    {
        private readonly ICurrentSite currentSite;
        private readonly IEmailSettingsService emailSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailContext" /> class.
        /// </summary>
        /// <param name="currentSite">The current site.</param>
        /// <param name="dataAccessFactory">The data access factory.</param>
        public EmailContext(ICurrentSite currentSite, IEmailSettingsService emailSettingsService)
        {
            this.emailSettingsService = emailSettingsService;
            this.currentSite = currentSite;

            this.LoadEmailSettings();
        }

        /// <summary>
        /// Gets or sets the email settings.
        /// </summary>
        /// <value>The email settings.</value>
        public EmailSettings EmailSettings { get; private set; }

        private void LoadEmailSettings()
        {
            if (this.currentSite == null)
                return;
            if (this.currentSite.Site == null)
                return;
            if (this.currentSite.Site.ChainId == 0)
                return;


            emailSettingsService.GetBestSettings(this.currentSite.Site.ChainId);
            //this.EmailSettings = dataAccessFactory.EmailCampaignDataAccess.GetEmailSettings(this.currentSite.Site.ChainId);

            ////set default settings
            //if (this.EmailSettings == null)
            //    this.LoadWebConfigSettings();
        }

        
    }
}

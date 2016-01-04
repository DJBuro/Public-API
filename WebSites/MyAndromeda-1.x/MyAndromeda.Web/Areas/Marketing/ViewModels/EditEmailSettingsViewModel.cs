using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.ViewModels
{
    public class EditEmailSettingsViewModel
    {
        public EditEmailSettingsViewModel() { }
        public EditEmailSettingsViewModel(EmailSettings settings)
        { 
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public EmailSettings Settings { get; set; }
    }
}
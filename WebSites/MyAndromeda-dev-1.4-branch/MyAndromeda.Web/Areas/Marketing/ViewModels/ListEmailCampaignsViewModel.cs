using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.ViewModels
{
    public class ListEmailCampaignsViewModel
    {
        public IEnumerable<EmailCampaign> Campaigns { get; set; }

        public Framework.Contexts.ICurrentSite CurrentSite { get; set; }
    }
}
using MyAndromeda.Web.Helper;
using System;

namespace MyAndromeda.Web.Areas.Marketing.ViewModels
{
    public class PreviewEmailCampaignViewModel
    {
        public int Id { get; set; }
        public string To { get; set; }
        public DateTime SendOn { get; set; }
        public EditEmailCampaignViewModel Template { get; set; }

        public bool DebugMode { get { return SettingsHelper.DebugMode; } }
        public string EmailTo { get { return SettingsHelper.DebugMailModeTo; } }
    }
}
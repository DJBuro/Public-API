using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Web.Areas.Marketing.Models
{
    public class PreviewMarketing
    {
        public MarketingEventCampaign Model { get; set; }
        public Preview Preview { get; set; }
    }

    public class Preview
    {
        public string To { get; set; }
        public bool Send { get; set; }
    }
}
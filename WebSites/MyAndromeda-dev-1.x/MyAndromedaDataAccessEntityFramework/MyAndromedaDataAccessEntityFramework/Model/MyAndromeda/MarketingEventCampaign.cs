//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAndromeda.Data.Model.MyAndromeda
{
    using System;
    using System.Collections.Generic;
    
    public partial class MarketingEventCampaign
    {
        public int AndromedaSiteId { get; set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public bool EnableEmail { get; set; }
        public bool EnableSms { get; set; }
        public string EmailTemplate { get; set; }
        public string SmsTemplate { get; set; }
        public string Preview { get; set; }
        public Nullable<System.DateTime> PreviewCreated { get; set; }
    }
}

using MyAndromeda.Core;
using System;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.SendGridService.MarketingEmails
{
    public interface IPreviewEmailCampaign : ITransientDependency
    {
        System.Threading.Tasks.Task<string> SendAsync(
            string outerTemplate, 
            string[] toAddresses, 
            MarketingEventCampaign model = null,
            bool send = true);
    }
}

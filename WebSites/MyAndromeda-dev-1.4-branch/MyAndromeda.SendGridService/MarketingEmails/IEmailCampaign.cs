using MyAndromeda.Core;
using System;
namespace MyAndromeda.SendGridService.MarketingEmails
{
    public interface IEmailCampaign : ITransientDependency
    {
        System.Threading.Tasks.Task Send();
    }
}

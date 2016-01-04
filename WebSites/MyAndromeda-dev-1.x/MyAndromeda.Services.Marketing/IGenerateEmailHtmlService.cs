using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Marketing
{
    public interface IGenerateEmailHtmlService : IDependency
    {
        string HtmlForWebApi(MarketingEventCampaign campaign);
        
        
        Task<string> HtmlForWebJob(MarketingEventCampaign campaign);
        //Task<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person> GetPeople(MarketingEventCampaign campaign);
    }
}

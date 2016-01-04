using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.Orders.Emails
{
    public interface ICommonEmailSetings 
    {
        Customer Customer { get; set; }

        Contact Contact { get; set; }

        OrderHeader Order { get; set; }
       
        Store Store { get; set; }

        AndroWebOrderingWebsite WebsiteStuff { get; set; }

        WebSiteConfigurations PreviewWebsiteConfiguration { get; }
        //{
        //    get 
        //    {
        //        return WebSiteConfigurations websiteConfigurations = WebSiteConfigurations.DeserializeJson(this.Model.WebsiteStuff.PreviewSettings);
        //    } 
        
        //}
    }
}
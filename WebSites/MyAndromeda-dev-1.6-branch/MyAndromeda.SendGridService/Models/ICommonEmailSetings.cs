using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.SendGridService.Models
{
    public interface ICommonEmailSetings 
    {
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
using MyAndromeda.Data.Model.MyAndromeda;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MyAndromedaSiteMediaServerService : IMyAndromedaSiteMediaServerDataService 
    {
        private readonly IMyAndromedaDbWorkContextAccessor dbWork;

        public MyAndromedaSiteMediaServerService(IMyAndromedaDbWorkContextAccessor dbWork)
        { 
            this.dbWork = dbWork;
        }

        public SiteMenuMediaServer GetDefault()
        {
            var table = this.dbWork.DbContext.SiteMenuMediaServers;
            var query = table.Where(e => e.Name.Equals("Default"));
            var result = query.SingleOrDefault();

            if (result == null)
            { 
                result = new SiteMenuMediaServer()
                {
                    Name = "Default",
                    Address = "http://cdn.myandromedaweb.co.uk/",
                    ContentPath = "http://{0}/menus/{1}/",
                    StoragePath = "menus/{0}"
                };
                this.dbWork.DbContext.SiteMenuMediaServers.Add(result);
                this.dbWork.DbContext.SaveChanges();
            }

            return result;
        }

        public SiteMenuMediaServer GetMediaServerWithDefault(int andomediaSiteId)
        {
            var table = this.dbWork.DbContext.SiteMenuMediaServers;
            var query = table.Where(e => e.SiteMenus.Any(siteMenu => siteMenu.AndromediaId == andomediaSiteId));
            var result = query.FirstOrDefault();

            if (result != null)
            {
                return result;
            }

            result = this.GetDefault();

            return result;
        }
    }
}
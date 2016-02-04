using System.Linq;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MyAndromedaMediaProfilesDataService : IMyAndromedaMediaProfilesDataService 
    {
        private readonly MyAndromedaDbContext dbContext;

        public MyAndromedaMediaProfilesDataService() 
        {
            this.dbContext = new MyAndromedaDbContext();
        }

        public IQueryable<SiteMenuMediaProfileSize> Query()
        {
            var table = this.dbContext.SiteMenuMediaProfileSizes;
            return table;
        }
    }
}
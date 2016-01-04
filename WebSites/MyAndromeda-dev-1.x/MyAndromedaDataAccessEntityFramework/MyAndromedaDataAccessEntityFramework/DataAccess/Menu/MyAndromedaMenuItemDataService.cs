using System.Linq;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Menu
{
    public class MyAndromedaMenuItemDataService : IMyAndromedaMenuItemDataService 
    {
        private readonly MyAndromedaDbContext dbContext;

        public MyAndromedaMenuItemDataService(MyAndromedaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IQueryable<MenuItem> Query(int andromedaSiteId)
        {
            var table = this.dbContext.MenuItems;
            var query = table.Where(e => e.SiteMenu.AndromediaId == andromedaSiteId);

            return query;
        }
    }
}